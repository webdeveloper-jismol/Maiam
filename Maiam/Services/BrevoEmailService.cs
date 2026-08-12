using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Maiam.Models;
using Maiam.Models.ViewModels;
using Microsoft.Extensions.Options;

namespace Maiam.Services
{
    public class BrevoEmailService : IEmailService
    {
        private const string BrevoEndpoint = "https://api.brevo.com/v3/smtp/email";
        private const string EnquirySubject = "Enquiry";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly EmailSettings _settings;
        private readonly ILogger<BrevoEmailService> _logger;

        public BrevoEmailService(IHttpClientFactory httpClientFactory, IOptions<EmailSettings> settings, ILogger<BrevoEmailService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<bool> SendContactEnquiryAsync(ContactViewModel enquiry)
        {
            if (string.IsNullOrWhiteSpace(_settings.BrevoApiKey) ||
                string.IsNullOrWhiteSpace(_settings.SenderEmail) ||
                string.IsNullOrWhiteSpace(_settings.RecipientEmail))
            {
                _logger.LogError("Brevo email settings are not fully configured (BrevoApiKey/SenderEmail/RecipientEmail). Enquiry from {Email} was not sent.", enquiry.Email);
                return false;
            }

            var payload = new
            {
                sender = new { name = _settings.SenderName, email = _settings.SenderEmail },
                to = new[] { new { email = _settings.RecipientEmail, name = "MAIAM" } },
                replyTo = new { email = enquiry.Email, name = enquiry.FullName },
                subject = EnquirySubject,
                htmlContent = BuildHtmlBody(enquiry)
            };

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("api-key", _settings.BrevoApiKey);

                var response = await client.PostAsJsonAsync(BrevoEndpoint, payload);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogError("Brevo email send failed with status {StatusCode}: {Body}", (int)response.StatusCode, responseBody);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while sending contact enquiry email via Brevo.");
                return false;
            }
        }

        private static string BuildHtmlBody(ContactViewModel enquiry)
        {
            string fullName = System.Net.WebUtility.HtmlEncode(enquiry.FullName);
            string phone = System.Net.WebUtility.HtmlEncode(enquiry.Phone);
            string email = System.Net.WebUtility.HtmlEncode(enquiry.Email);
            string course = System.Net.WebUtility.HtmlEncode(enquiry.InterestedCourse);
            string message = System.Net.WebUtility.HtmlEncode(enquiry.Message)
                .Replace("\r\n", "<br />")
                .Replace("\n", "<br />");

            return $@"
<h2>New Enquiry - MAIAM Website</h2>
<p><strong>Full Name:</strong> {fullName}</p>
<p><strong>Phone:</strong> {phone}</p>
<p><strong>Email:</strong> {email}</p>
<p><strong>Interested Course:</strong> {course}</p>
<p><strong>Message:</strong></p>
<p>{message}</p>
<hr />
<p>This enquiry was submitted through the MAIAM website.</p>";
        }
    }
}
