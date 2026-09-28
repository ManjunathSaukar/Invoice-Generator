using Dapper;
using InvoiceAPI.Constants;
using InvoiceAPI.Entities;
using InvoiceAPI.Interfaces;

namespace InvoiceAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDapperRepository _dapperRepository;

        public ProductRepository(IDapperRepository dapperRepository)
        {
            _dapperRepository = dapperRepository;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _dapperRepository.QueryAsync<Product>(
                StoredProcedures.GetProducts);
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@ProductId", productId);

            return await _dapperRepository.QueryFirstOrDefaultAsync<Product>(
                StoredProcedures.GetProductById,
                parameters);
        }
    }
}
