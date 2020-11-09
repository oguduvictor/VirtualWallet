using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VirtualWallet.Application.Interfaces.Repositories;
using VirtualWallet.Domain.Entities;
using VirtualWallet.Infrastructure.Persistence.Contexts;
using VirtualWallet.Infrastructure.Persistence.Repository;

namespace VirtualWallet.Infrastructure.Persistence.Repositories
{
    public class ProductRepositoryAsync : GenericRepositoryAsync<Product>, IProductRepositoryAsync
    {
        private readonly DbSet<Product> _products;

        public ProductRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _products = dbContext.Set<Product>();
        }

        public Task<bool> IsUniqueBarcodeAsync(string barcode)
        {
            return _products
                .AllAsync(p => p.Barcode != barcode);
        }
    }
}
