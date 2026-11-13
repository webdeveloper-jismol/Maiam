using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;
using Maiamnew.Models;
using Microsoft.Extensions.Options;

namespace Maiamnew.Services
{
    /// <summary>
    /// SMTP settings, read from the "Email" configuration section. Credentials belong in
    /// user secrets (development) or environment variables / server configuration (IIS),
    /// never in source control. See docs/EMAIL_SETUP.md.
    /// </summary>
    public class EmailSettings
    {
        public string? Host { get; set; }
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? FromAddress { get; set; }
        public string FromName { get; set; } = "MAIAM Website";

        /// <summary>Where Register Now enquiries are delivered.</summary>
        public string ToAddress { get; set; } = "maiam@manappuramfoundation.org";

        /// <summary>Where Contact Us messages are delivered.</summary>
        public string ContactToAddress { get; set; } = "mafoundteamit@gmail.com";

        /// <summary>Development only: write messages as .eml files to this folder instead of sending.</summary>
        public string? PickupDirectory { get; set; }

        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(PickupDirectory)
            || (!string.IsNullOrWhiteSpace(Host) && !string.IsNullOrWhiteSpace(FromAddress));
    }

    public interface IEnquiryEmailSender
    {
        bool IsConfigured { get; }

        /// <returns>true when the message was handed to the mail server.</returns>
        Task<bool> SendAsync(RegisterEnquiry enquiry, CancellationToken cancellationToken);

        /// <returns>true when the message was handed to the mail server.</returns>
        Task<bool> SendAsync(ContactMessage message, CancellationToken cancellationToken);
    }

    /// <remarks>
    /// Takes <see cref="IOptionsSnapshot{T}"/> rather than <see cref="IOptions{T}"/> so that edits to the
    /// "Email" section of appsettings.json take effect on the next request instead of needing a restart.
    /// The service is registered as scoped, so the settings are read once per request.
    /// </remarks>
    public class SmtpEnquiryEmailSender(IOptionsSnapshot<EmailSettings> options, ILogger<SmtpEnquiryEmailSender> logger) : IEnquiryEmailSender
    {
        private readonly EmailSettings _settings = options.Value;

        public bool IsConfigured => _settings.IsConfigured;

        public async Task<bool> SendAsync(RegisterEnquiry enquiry, CancellationToken cancellationToken)
        {
            if (!IsConfigured)
            {
                logger.LogWarning("Register Now enquiry not sent: Email settings are not configured.");
                return false;
            }

            try
            {
                using var message = BuildMessage(enquiry);
                using var client = CreateClient();
                await client.SendMailAsync(message, cancellationToken);
                logger.LogInformation("Register Now enquiry sent to {To}.", _settings.ToAddress);
                return true;
            }
            catch (Exception ex) when (ex is SmtpException or InvalidOperationException or FormatException or IOException)
            {
                logger.LogError(ex, "Register Now enquiry could not be sent.");
                return false;
            }
        }

        public async Task<bool> SendAsync(ContactMessage contact, CancellationToken cancellationToken)
        {
            if (!IsConfigured)
            {
                logger.LogWarning("Contact Us message not sent: Email settings are not configured.");
                return false;
            }

            try
            {
                using var message = BuildMessage(contact);
                using var client = CreateClient();
                await client.SendMailAsync(message, cancellationToken);
                logger.LogInformation("Contact Us message sent to {To}.", _settings.ContactToAddress);
                return true;
            }
            catch (Exception ex) when (ex is SmtpException or InvalidOperationException or FormatException or IOException)
            {
                logger.LogError(ex,
                    "Contact Us message could not be sent to {To} through {Host}:{Port}. An authentication error " +
                    "means Email:Password is not valid for this account; a timeout usually means outbound port " +
                    "{Port} is blocked on this server.",
                    _settings.ContactToAddress, _settings.Host, _settings.Port, _settings.Port);
                return false;
            }
        }

        private SmtpClient CreateClient()
        {
            if (!string.IsNullOrWhiteSpace(_settings.PickupDirectory))
            {
                Directory.CreateDirectory(_settings.PickupDirectory);
                return new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    PickupDirectoryLocation = _settings.PickupDirectory,
                };
            }

            var client = new SmtpClient(_settings.Host, _settings.Port) { EnableSsl = _settings.EnableSsl };
            if (!string.IsNullOrWhiteSpace(_settings.UserName))
            {
                client.Credentials = new NetworkCredential(_settings.UserName, _settings.Password);
            }
            return client;
        }

        private MailMessage BuildMessage(RegisterEnquiry e)
        {
            const string formName = "Register Now form";
            var submittedOn = EnquiryEmailTemplate.SubmittedOn();
            var fields = new[]
            {
                new EnquiryEmailTemplate.Field("Name of the Student", e.StudentName),
                new EnquiryEmailTemplate.Field("Email", e.Email, IsEmail: true),
                new EnquiryEmailTemplate.Field("Phone", e.ContactNumber),
                new EnquiryEmailTemplate.Field("Age", e.Age?.ToString(CultureInfo.InvariantCulture)),
                new EnquiryEmailTemplate.Field("Qualification", e.Qualification),
                new EnquiryEmailTemplate.Field("Course Interested In", e.Course),
                new EnquiryEmailTemplate.Field("Address", e.Address, MultiLine: true),
            };

            var message = NewMessage(
                subject: $"New Course Enquiry – {OneLine(e.StudentName, 60)}",
                heading: "New Course Enquiry",
                formName: formName,
                fields: fields,
                body: e.Message ?? string.Empty,
                submittedOn: submittedOn,
                to: _settings.ToAddress);

            if (!string.IsNullOrWhiteSpace(e.Email))
            {
                message.ReplyToList.Add(new MailAddress(e.Email.Trim(), OneLine(e.StudentName, 60)));
            }
            return message;
        }

        private MailMessage BuildMessage(ContactMessage c)
        {
            const string formName = "Contact Us form";
            var fields = new[]
            {
                new EnquiryEmailTemplate.Field("Name", c.Name),
                new EnquiryEmailTemplate.Field("Email", c.Email, IsEmail: true),
                new EnquiryEmailTemplate.Field("Phone", c.Phone),
                new EnquiryEmailTemplate.Field("Course Interested In", c.Course),
            };

            var message = NewMessage(
                subject: $"New Website Enquiry – {OneLine(c.Name, 60)}",
                heading: "New Website Enquiry",
                formName: formName,
                fields: fields,
                body: c.Message,
                submittedOn: EnquiryEmailTemplate.SubmittedOn(),
                to: _settings.ContactToAddress);

            // The visitor is the Reply-To, never the From, so the message is genuinely sent by the
            // site's own mailbox and still passes SPF/DKIM, while Reply goes to the visitor.
            message.ReplyToList.Add(new MailAddress(c.Email.Trim(), OneLine(c.Name, 60)));
            return message;
        }

        /// <summary>
        /// Builds a multipart message carrying both a plain-text and an HTML body, so clients that
        /// cannot (or will not) render HTML still show a readable enquiry.
        /// </summary>
        private MailMessage NewMessage(
            string subject,
            string heading,
            string formName,
            IReadOnlyList<EnquiryEmailTemplate.Field> fields,
            string body,
            string submittedOn,
            string to)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_settings.FromAddress ?? "no-reply@maiam.co.in", _settings.FromName),
                // Subject: single line, no user-controlled line breaks.
                Subject = subject,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8,
            };
            message.To.Add(to);

            var text = EnquiryEmailTemplate.Text(heading, formName, fields, body, submittedOn);
            var html = EnquiryEmailTemplate.Html(heading, formName, fields, body, submittedOn);

            // Least-preferred alternative first; mail clients pick the last one they can display.
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, Encoding.UTF8, "text/plain"));
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, "text/html"));
            return message;
        }

        private static string OneLine(string value, int max)
        {
            var single = string.Join(' ', value.Split(['\r', '\n', '\t'], StringSplitOptions.RemoveEmptyEntries)).Trim();
            return single.Length > max ? single[..max] : single;
        }
    }
}
