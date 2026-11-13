using System.ComponentModel.DataAnnotations;

namespace Maiamnew.Models
{
    /// <summary>
    /// "Send us a message" form on the Contact Us page. Messages are emailed to the
    /// address in Email:ContactToAddress (see docs/EMAIL_SETUP.md).
    /// </summary>
    public class ContactMessage
    {
        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        // Latin, accented Latin and Malayalam letters, spaces, dots, apostrophes and hyphens.
        // Explicit ranges (not \p{L}) so the same pattern works in browser validation.
        [RegularExpression(@"^[A-Za-zÀ-ɏഀ-ൿ][A-Za-zÀ-ɏഀ-ൿ .'\-]*$",
            ErrorMessage = "Name can contain letters, spaces, dots, apostrophes and hyphens only.")]
        [Display(Name = "Full name")]
        public string Name { get; set; } = string.Empty;

        // A bare address only: it becomes the email's Reply-To, so display names,
        // angle brackets, commas and line breaks are rejected here.
        [Required(ErrorMessage = "Please enter your email address.")]
        [StringLength(254, ErrorMessage = "Email must be 254 characters or fewer.")]
        [RegularExpression(@"^[A-Za-z0-9._%+'\-]+@[A-Za-z0-9\-]+(?:\.[A-Za-z0-9\-]+)*\.[A-Za-z]{2,}$",
            ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a contact number.")]
        [RegularExpression(@"^(?:\+?91[\s-]?|0)?[6-9]\d{4}[\s-]?\d{5}$",
            ErrorMessage = "Please enter a valid 10-digit Indian mobile number.")]
        [Display(Name = "Phone number")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Course of interest (optional)")]
        public string? Course { get; set; }

        [Required(ErrorMessage = "Please enter your message.")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Message must be between 5 and 1000 characters.")]
        [Display(Name = "Message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>Honeypot: hidden from people, often filled in by bots.</summary>
        public string? Website { get; set; }

        /// <summary>Protected timestamp of when the form was rendered (minimum fill time check).</summary>
        public string? FormToken { get; set; }
    }
}
