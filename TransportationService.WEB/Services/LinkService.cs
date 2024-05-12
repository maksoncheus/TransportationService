using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using TransportationService.WEB.Data.Entities;

namespace TransportationService.WEB.Services
{
    public class LinkService
    {
        private readonly UserManager<User> _userManager;
        public LinkService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> GenerateEmailConfirmationLink(User user, string urlTo)
        {
            string confirmationToken = await _userManager
                 .GenerateEmailConfirmationTokenAsync(user);

            string? confirmationLink = urlTo + $"?userid={user.Id}&token={confirmationToken}";
            if (confirmationLink == null) throw new ArgumentNullException(nameof(confirmationLink));
            return confirmationLink;
        }
        public async Task<string> GenerateResetPasswordLink(User user, string urlTo)
        {
            string resetToken = await _userManager
                 .GeneratePasswordResetTokenAsync(user);

            string? confirmationLink = urlTo + $"?userid={user.Id}&token={resetToken}";
            if (confirmationLink == null) throw new ArgumentNullException(nameof(confirmationLink));
            return confirmationLink;
        }
    }
}
