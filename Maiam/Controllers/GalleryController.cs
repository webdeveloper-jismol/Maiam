using Maiam.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Maiam.Controllers
{
    public class GalleryController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Gallery";
            ViewData["Description"] = "Photo gallery of MAIAM's workshops, training sessions, student activities and campus life.";

            var model = new GalleryViewModel
            {
                Images = new List<string>
                {
                    "/images/gallery/gallery-1.jpg",
                    "/images/gallery/gallery-2.jpg",
                    "/images/gallery/gallery-3.jpg",
                    "/images/gallery/gallery-4.png",
                    "/images/gallery/gallery-5.jpg",
                    "/images/gallery/gallery-6.jpg",
                    "/images/gallery/gallery-7.jpg",
                    "/images/gallery/gallery-8.jpg",
                    "/images/home/hero/slide-3.jpg",
                    "/images/home/hero/slide-4.jpg",
                }
            };

            return View(model);
        }
    }
}
