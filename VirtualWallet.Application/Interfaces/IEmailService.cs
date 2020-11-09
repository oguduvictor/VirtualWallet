using System.Threading.Tasks;
using VirtualWallet.Application.DTOs.Email;

namespace VirtualWallet.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
