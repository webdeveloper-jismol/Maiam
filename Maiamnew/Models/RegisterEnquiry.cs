using System.ComponentModel.DataAnnotations;

namespace Maiamnew.Models
{
    /// <summary>
    /// "Register Now" enquiry. The first five fields are the original WordPress form fields;
    /// email, course and message are optional additions.
    /// </summary>
    public class RegisterEnquiry
    {
        [Required(ErrorMessage = "Please enter the student’s name.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        // Latin, accented Latin and Malayalam letters. Explicit ranges (not \p{L}) so the same
        // pattern also works in the browser's client-side validation.
        [RegularExpression(@"^[A-Za-zÀ-ɏഀ-ൿ][A-Za-zÀ-ɏഀ-ൿ .'\-]*$", ErrorMessage = "Name can contain letters, spaces, dots, apostrophes and hyphens only.")]
        [Display(Name = "Name of the student")]
        public string StudentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the address.")]
        [StringLength(400, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 400 characters.")]
        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the educational qualification.")]
        [StringLength(100, ErrorMessage = "Qualification must be 100 characters or fewer.")]
        [Display(Name = "Educational qualification")]
        public string Qualification { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a contact number.")]
        [RegularExpression(@"^(?:\+?91[\s-]?|0)?[6-9]\d{4}[\s-]?\d{5}$", ErrorMessage = "Please enter a valid 10-digit Indian mobile number.")]
        [Display(Name = "Contact number")]
        public string ContactNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the age.")]
        [Range(14, 60, ErrorMessage = "Please enter an age between 14 and 60.")]
        [Display(Name = "Age")]
        public int? Age { get; set; }

        // A bare address only: it becomes the email's Reply-To, so display names, angle brackets,
        // commas and spaces are rejected here. [EmailAddress] is too permissive for that — it
        // accepts "Name <a@b.com>" and "a@b c", which would either smuggle a display name into the
        // header or throw when the address is parsed, losing the enquiry.
        [StringLength(150, ErrorMessage = "Email must be 150 characters or fewer.")]
        [RegularExpression(@"^[A-Za-z0-9._%+'\-]+@[A-Za-z0-9\-]+(?:\.[A-Za-z0-9\-]+)*\.[A-Za-z]{2,}$",
            ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email address (optional)")]
        public string? Email { get; set; }

        [StringLength(100)]
        [Display(Name = "Course of interest (optional)")]
        public string? Course { get; set; }

        [StringLength(1000, ErrorMessage = "Message must be 1000 characters or fewer.")]
        [Display(Name = "Comments or questions (optional)")]
        public string? Message { get; set; }

        /// <summary>Honeypot: hidden from people, often filled in by bots.</summary>
        public string? Website { get; set; }

        /// <summary>Protected timestamp of when the form was rendered (minimum fill time check).</summary>
        public string? FormToken { get; set; }
    }
}
