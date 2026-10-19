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
        public string ToAddress { get; set; } = "maiam@manappuramfoundation.org";

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
    }

    public class SmtpEnquiryEmailSender(IOptions<EmailSettings> options, ILogger<SmtpEnquiryEmailSender> logger) : IEnquiryEmailSender
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
            var from = new MailAddress(_settings.FromAddress ?? "no-reply@maiam.co.in", _settings.FromName);
            var message = new MailMessage
            {
                From = from,
                // Subject: single line, no user-controlled line breaks.
                Subject = $"Register Now enquiry – {OneLine(e.StudentName, 60)}",
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = false,
                Body = BuildBody(e),
            };
            message.To.Add(_settings.ToAddress);
            if (!string.IsNullOrWhiteSpace(e.Email))
            {
                message.ReplyToList.Add(new MailAddress(e.Email.Trim(), OneLine(e.StudentName, 60)));
            }
            return message;
        }

        private static string BuildBody(RegisterEnquiry e)
        {
            var body = new StringBuilder();
            body.AppendLine("New enquiry from the MAIAM website Register Now form.");
            body.AppendLine();
            body.AppendLine($"Name of the student:       {OneLine(e.StudentName, 100)}");
            body.AppendLine($"Address:                   {e.Address.Trim()}");
            body.AppendLine($"Educational qualification: {OneLine(e.Qualification, 100)}");
            body.AppendLine($"Contact number:            {OneLine(e.ContactNumber, 20)}");
            body.AppendLine($"Age:                       {e.Age}");
            body.AppendLine($"Email:                     {OneLine(e.Email ?? "-", 150)}");
            body.AppendLine($"Course of interest:        {OneLine(e.Course ?? "-", 100)}");
            body.AppendLine();
            body.AppendLine("Comments or questions:");
            body.AppendLine(string.IsNullOrWhiteSpace(e.Message) ? "-" : e.Message.Trim());
            body.AppendLine();
            body.AppendLine($"Submitted: {DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(5.5)):dd MMM yyyy, HH:mm} IST");
            return body.ToString();
        }

        private static string OneLine(string value, int max)
        {
            var single = string.Join(' ', value.Split(['\r', '\n', '\t'], StringSplitOptions.RemoveEmptyEntries)).Trim();
            return single.Length > max ? single[..max] : single;
        }
    }
}
