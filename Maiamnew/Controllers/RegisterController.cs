using System.Globalization;
using Maiamnew.Models;
using Maiamnew.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Maiamnew.Controllers
{
    /// <summary>"Register Now" enquiry form (original URL /register-now/), delivered by email.</summary>
    public class RegisterController : Controller
    {
        private static readonly TimeSpan MinimumFillTime = TimeSpan.FromSeconds(2);

        private readonly IEnquiryEmailSender _sender;
        private readonly IDataProtector _protector;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(IEnquiryEmailSender sender, IDataProtectionProvider protection, ILogger<RegisterController> logger)
        {
            _sender = sender;
            _protector = protection.CreateProtector("Maiamnew.RegisterNow.FormToken");
            _logger = logger;
        }

        [HttpGet]
        [Route("register-now")]
        public IActionResult RegisterNow()
        {
            return View(new RegisterEnquiry { FormToken = NewFormToken() });
        }

        [HttpPost]
        [Route("register-now")]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("forms")]
        public async Task<IActionResult> RegisterNow(RegisterEnquiry model, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(model.Website) || !FilledByPerson(model.FormToken))
            {
                _logger.LogWarning("Register Now submission rejected by spam checks.");
                ModelState.AddModelError(string.Empty, "Your submission could not be processed. Please try again, or call us.");
            }

            if (!ModelState.IsValid)
            {
                model.FormToken = NewFormToken();
                return View(model);
            }

            var sent = await _sender.SendAsync(model, cancellationToken);
            if (!sent)
            {
                // Never report success unless the enquiry actually reached the mail server.
                ViewData["SendFailed"] = true;
                model.FormToken = NewFormToken();
                return View(model);
            }

            TempData["EnquirySent"] = true;
            return Redirect("/register-now/#enquiry-status");
        }

        private string NewFormToken() =>
            _protector.Protect(DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture));

        private bool FilledByPerson(string? token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            try
            {
                var rendered = new DateTime(long.Parse(_protector.Unprotect(token), CultureInfo.InvariantCulture), DateTimeKind.Utc);
                return DateTime.UtcNow - rendered >= MinimumFillTime;
            }
            catch (Exception ex) when (ex is System.Security.Cryptography.CryptographicException or FormatException or ArgumentOutOfRangeException)
            {
                return false;
            }
        }
    }
}
