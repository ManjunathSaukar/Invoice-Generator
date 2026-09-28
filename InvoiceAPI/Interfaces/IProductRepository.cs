using InvoiceAPI.Entities;

namespace InvoiceAPI.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();

        Task<Product?> GetProductByIdAsync(int productId);
    }
}
