using System.ComponentModel.DataAnnotations;

namespace Maiam.Models.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Please enter your full name.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your phone number.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a course.")]
        [Display(Name = "Interested Course")]
        public string InterestedCourse { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a message.")]
        public string Message { get; set; } = string.Empty;
    }
}
