using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualWallet.Domain.Entities;

namespace VirtualWallet.Infrastructure.Persistence.Configurations
{
    internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasOne(x => x.Wallet).WithMany().HasForeignKey(x => x.WalletId);
        }
    }
}
