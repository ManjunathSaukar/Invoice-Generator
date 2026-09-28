using InvoiceAPI.DTOs.Response;
using InvoiceAPI.Entities;

namespace InvoiceAPI.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetProductsAsync();

        Task<ProductResponse?> GetProductByIdAsync(int productId);
    }
}
