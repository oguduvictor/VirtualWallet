using System.ComponentModel.DataAnnotations;

namespace VirtualWallet.Application.DTOs.Account
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
