using Maiam.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Maiam.Controllers
{
    public class GalleryController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Gallery";
            ViewData["Description"] = "Photo gallery of MAIAM's events, workshops, and campus life - Appointment Letter Distribution, Toyota Plant Visit, Drug Awareness Programme, Onam Celebrations and the Workshop Inauguration.";

            var model = new GalleryViewModel
            {
                Sections = new List<GallerySection>
                {
                    new()
                    {
                        Heading = "Appointment Letter Distribution Ceremony",
                        Images = new List<GalleryImage>
                        {
                            new() { Url = "/images/gallery/appointment-letter/appointment-letter-01.webp", Alt = "Appointment Letter Distribution Ceremony - MAIAM" },
                        }
                    },
                    new()
                    {
                        Heading = "TOYOTA PLANT VISIT",
                        Images = new List<GalleryImage>
                        {
                            new() { Url = "/images/gallery/toyota-plant-visit/toyota-plant-01.webp", Alt = "Toyota Plant Visit - MAIAM" },
                        }
                    },
                    new()
                    {
                        Heading = "Drug Awareness Section",
                        Images = new List<GalleryImage>
                        {
                            new() { Url = "/images/gallery/drug-awareness/drug-awareness-01.webp", Alt = "Drug Awareness Programme - MAIAM" },
                            new() { Url = "/images/gallery/drug-awareness/drug-awareness-02.webp", Alt = "Drug Awareness Programme - MAIAM" },
                            new() { Url = "/images/gallery/drug-awareness/drug-awareness-03.webp", Alt = "Drug Awareness Programme - MAIAM" },
                        }
                    },
                    new()
                    {
                        Heading = "Onam Celebrations 2024",
                        Images = new List<GalleryImage>
                        {
                            new() { Url = "/images/gallery/onam-2024/onam-2024-01.jpg", Alt = "Onam Celebrations 2024 - MAIAM" },
                            new() { Url = "/images/gallery/onam-2024/onam-2024-02.jpg", Alt = "Onam Celebrations 2024 - MAIAM" },
                            new() { Url = "/images/gallery/onam-2024/onam-2024-03.jpg", Alt = "Onam Celebrations 2024 - MAIAM" },
                            new() { Url = "/images/gallery/onam-2024/onam-2024-04.jpg", Alt = "Onam Celebrations 2024 - MAIAM" },
                            new() { Url = "/images/gallery/onam-2024/onam-2024-05.jpg", Alt = "Onam Celebrations 2024 - MAIAM" },
                            new() { Url = "/images/gallery/onam-2024/onam-2024-06.jpg", Alt = "Onam Celebrations 2024 - MAIAM" },
                            new() { Url = "/images/gallery/onam-2024/onam-2024-07.jpg", Alt = "Onam Celebrations 2024 - MAIAM" },
                            new() { Url = "/images/gallery/onam-2024/onam-2024-08.jpg", Alt = "Onam Celebrations 2024 - MAIAM" },
                            new() { Url = "/images/gallery/onam-2024/onam-2024-09.jpg", Alt = "Onam Celebrations 2024 - MAIAM" },
                            new() { Url = "/images/gallery/onam-2024/onam-2024-10.jpg", Alt = "Onam Celebrations 2024 - MAIAM" },
                        }
                    },
                    new()
                    {
                        Heading = "Renovated Office Building, Workshop & Classroom Inauguration",
                        Images = new List<GalleryImage>
                        {
                            new() { Url = "/images/gallery/inauguration/inauguration-01.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-02.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-03.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-04.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-05.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-06.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-07.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-08.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-09.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-10.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-11.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-12.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-13.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-14.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-15.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-16.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-17.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-18.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-19.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-20.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-21.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-22.jpeg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-23.jpeg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-24.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-25.jpeg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-26.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-27.jpeg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-28.jpeg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-29.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-30.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-31.jpeg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-32.jpeg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-33.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-34.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-35.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-36.jpeg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-37.jpeg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-38.jpeg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-39.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-40.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-41.jpeg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-42.jpeg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-43.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-44.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-45.jpeg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-46.png", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-47.jpeg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-48.jpeg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-49.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-50.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-51.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-52.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-53.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-54.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-55.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-56.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-57.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-58.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-59.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-60.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-61.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-62.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-63.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-64.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-65.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-66.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-67.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                            new() { Url = "/images/gallery/inauguration/inauguration-68.jpg", Alt = "MAIAM Workshop and Classroom Inauguration" },
                        }
                    },
                }
            };

            return View(model);
        }
    }
}
