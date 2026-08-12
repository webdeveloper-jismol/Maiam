using Maiam.Models.ViewModels;
using Maiam.Services;
using Microsoft.AspNetCore.Mvc;

namespace Maiam.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IEmailService emailService, ILogger<ContactController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            SetMetadata();
            return View(new ContactViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactViewModel model)
        {
            SetMetadata();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool sent;
            try
            {
                sent = await _emailService.SendContactEnquiryAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error while processing contact enquiry submission from {Email}.", model.Email);
                sent = false;
            }

            if (sent)
            {
                // Post/Redirect/Get: prevents duplicate submissions on browser refresh.
                TempData["SuccessMessage"] = "Thank you! Your enquiry has been submitted successfully.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["ErrorMessage"] = "Unable to send your enquiry at the moment. Please try again later.";
            return View(model);
        }

        private void SetMetadata()
        {
            ViewData["Title"] = "Contact";
            ViewData["Description"] = "Get in touch with MAIAM for admissions, course enquiries or campus visits. Call, email or send us a message.";
        }
    }
}
