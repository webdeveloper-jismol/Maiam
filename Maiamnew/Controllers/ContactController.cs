using Maiamnew.Infrastructure;
using Maiamnew.Models;
using Maiamnew.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Maiamnew.Controllers
{
    public class ContactController(
        IEnquiryEmailSender sender,
        FormGuard formGuard,
        ILogger<ContactController> logger) : Controller
    {
        [HttpGet]
        [Route("contact-us")]
        public IActionResult Contact()
        {
            return View(new ContactMessage { FormToken = formGuard.NewToken() });
        }

        [HttpPost]
        [Route("contact-us")]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("forms")]
        public async Task<IActionResult> Contact(ContactMessage model, CancellationToken cancellationToken)
        {
            // Normalise before validating, so input made only of spaces fails the Required checks
            // and no stray control characters reach the email.
            Normalise(model);
            ModelState.Clear();
            TryValidateModel(model);

            if (!string.IsNullOrEmpty(model.Website) || !formGuard.FilledByPerson(model.FormToken))
            {
                logger.LogWarning("Contact Us submission rejected by spam checks.");
                ModelState.AddModelError(string.Empty, "Your message could not be processed. Please try again, or call us.");
            }

            if (!ModelState.IsValid)
            {
                model.FormToken = formGuard.NewToken();
                return View(model);
            }

            var sent = await sender.SendAsync(model, cancellationToken);
            if (!sent)
            {
                // Never report success unless the message actually reached the mail server.
                ViewData["SendFailed"] = true;
                model.FormToken = formGuard.NewToken();
                return View(model);
            }

            TempData["ContactSent"] = true;
            return Redirect("/contact-us/#message-status");
        }

        private static void Normalise(ContactMessage model)
        {
            model.Name = FormText.SingleLine(model.Name) ?? string.Empty;
            model.Email = FormText.SingleLine(model.Email) ?? string.Empty;
            model.Phone = FormText.SingleLine(model.Phone) ?? string.Empty;
            model.Course = FormText.SingleLine(model.Course);
            model.Message = FormText.MultiLine(model.Message);
        }
    }
}
