using VirtualWallet.Application.Interfaces.Repositories;
using VirtualWallet.Domain.Entities;
using VirtualWallet.Infrastructure.Persistence.Contexts;
using VirtualWallet.Infrastructure.Persistence.Repository;

namespace VirtualWallet.Infrastructure.Persistence.Repositories
{
    public class TransactionRepositoryAsync : GenericRepositoryAsync<Transaction>, ITransactionRepositoryAsync
    {
        public TransactionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
