using InvoiceAPI.DTOs.Response;
using InvoiceAPI.Entities;
using InvoiceAPI.Interfaces;

namespace InvoiceAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductResponse>> GetProductsAsync()
        {
            var products = await _productRepository.GetProductsAsync();

            return products.Select(product => new ProductResponse
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                IsImported = product.IsImported
            });
        }

        public async Task<ProductResponse?> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product == null)
                return null;

            return new ProductResponse
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                IsImported = product.IsImported
            };
        }
    }
}
