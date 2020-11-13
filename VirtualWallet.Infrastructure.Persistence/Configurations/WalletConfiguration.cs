using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualWallet.Domain.Entities;

namespace VirtualWallet.Infrastructure.Persistence.Configurations
{
    internal class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasIndex(c => c.AccountNumber).IsUnique();
            builder.HasIndex(c => c.WalletOwnerId).IsUnique();
        }
    }
}
