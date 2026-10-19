using Microsoft.AspNetCore.Mvc;

namespace Maiamnew.Controllers
{
    /// <summary>"About Us › Certifications" pages, served at their original WordPress URLs.</summary>
    public class CertificationsController : Controller
    {
        [Route("nsdc")]
        public IActionResult Nsdc() => View();

        [Route("certificate-of-affiliation")]
        public IActionResult AffiliationCertificate() => View();
    }
}
