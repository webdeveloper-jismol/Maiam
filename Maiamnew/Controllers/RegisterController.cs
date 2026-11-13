using Maiamnew.Infrastructure;
using Maiamnew.Models;
using Maiamnew.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Maiamnew.Controllers
{
    /// <summary>"Register Now" enquiry form (original URL /register-now/), delivered by email.</summary>
    public class RegisterController : Controller
    {
        private readonly IEnquiryEmailSender _sender;
        private readonly FormGuard _formGuard;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(IEnquiryEmailSender sender, FormGuard formGuard, ILogger<RegisterController> logger)
        {
            _sender = sender;
            _formGuard = formGuard;
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
            // Normalise before validating, so input made only of spaces fails the Required checks
            // and no stray control characters reach the email.
            Normalise(model);
            ModelState.Clear();
            TryValidateModel(model);

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

        private static void Normalise(RegisterEnquiry model)
        {
            model.StudentName = FormText.SingleLine(model.StudentName) ?? string.Empty;
            model.Qualification = FormText.SingleLine(model.Qualification) ?? string.Empty;
            model.ContactNumber = FormText.SingleLine(model.ContactNumber) ?? string.Empty;
            model.Email = FormText.SingleLine(model.Email);
            model.Course = FormText.SingleLine(model.Course);
            model.Address = FormText.MultiLine(model.Address);
            model.Message = FormText.MultiLine(model.Message);
        }

        private string NewFormToken() => _formGuard.NewToken();

        private bool FilledByPerson(string? token) => _formGuard.FilledByPerson(token);
    }
}
