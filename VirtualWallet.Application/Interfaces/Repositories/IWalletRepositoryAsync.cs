using System.Threading.Tasks;
using VirtualWallet.Domain.Entities;

namespace VirtualWallet.Application.Interfaces.Repositories
{
    public interface IWalletRepositoryAsync : IGenericRepositoryAsync<Wallet>
    {
        public Task<bool> AccountNumberExistsAsync(long accountNumber);
        public Task<Wallet> GetByWalletOwnerIdAsync(string walletOwnerId);
        public Task<Wallet> GetByAccountNumberAsync(long accountNumber);
    }
}
