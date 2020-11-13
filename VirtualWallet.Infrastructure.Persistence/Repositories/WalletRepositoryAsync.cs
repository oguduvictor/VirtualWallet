using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VirtualWallet.Application.Interfaces.Repositories;
using VirtualWallet.Domain.Entities;
using VirtualWallet.Infrastructure.Persistence.Contexts;
using VirtualWallet.Infrastructure.Persistence.Repository;

namespace VirtualWallet.Infrastructure.Persistence.Repositories
{
    public class WalletRepositoryAsync : GenericRepositoryAsync<Wallet>, IWalletRepositoryAsync
    {
        private readonly ApplicationDbContext _dbContext;

        public WalletRepositoryAsync(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AccountNumberExistsAsync(long accountNumber)
        {
            return await _dbContext.Wallets.AnyAsync(x => x.AccountNumber == accountNumber);
        }

        public async Task<Wallet> GetByAccountNumberAsync(long accountNumber)
        {
            return await _dbContext.Wallets.SingleOrDefaultAsync(x => x.AccountNumber == accountNumber);
        }

        public async Task<Wallet> GetByWalletOwnerIdAsync(string walletOwnerId)
        {
            return await _dbContext.Wallets.SingleOrDefaultAsync(x => x.WalletOwnerId == walletOwnerId);
        }
    }
}
