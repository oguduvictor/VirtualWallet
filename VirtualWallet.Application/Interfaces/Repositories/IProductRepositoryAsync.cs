using System.Threading.Tasks;
using VirtualWallet.Domain.Entities;

namespace VirtualWallet.Application.Interfaces.Repositories
{
    public interface IProductRepositoryAsync : IGenericRepositoryAsync<Product>
    {
        Task<bool> IsUniqueBarcodeAsync(string barcode);
    }
}
