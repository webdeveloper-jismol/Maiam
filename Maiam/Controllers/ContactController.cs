using Maiam.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Maiam.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Contact";
            ViewData["Description"] = "Get in touch with MAIAM for admissions, course enquiries or campus visits. Call, email or send us a message.";

            return View(new ContactViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ContactViewModel model)
        {
            ViewData["Title"] = "Contact";
            ViewData["Description"] = "Get in touch with MAIAM for admissions, course enquiries or campus visits. Call, email or send us a message.";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // NOTE: No email/database integration is wired up yet. The form validates
            // and submits successfully, but the message is not currently sent or stored anywhere.
            ViewData["SubmitSuccess"] = true;
            ModelState.Clear();
            return View(new ContactViewModel());
        }
    }
}
