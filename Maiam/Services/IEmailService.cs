using Maiam.Models.ViewModels;

namespace Maiam.Services
{
    public interface IEmailService
    {
        Task<bool> SendContactEnquiryAsync(ContactViewModel enquiry);
    }
}
