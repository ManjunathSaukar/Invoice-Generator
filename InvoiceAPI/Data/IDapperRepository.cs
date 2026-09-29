using Dapper;

namespace InvoiceAPI.Data
{
    public interface IDapperRepository
    {
        Task<IEnumerable<T>> QueryAsync<T>(
            string storedProcedure,
            DynamicParameters? parameters = null);

        Task<T?> QueryFirstOrDefaultAsync<T>(
            string storedProcedure,
            DynamicParameters? parameters = null);

        Task<int> ExecuteAsync(
            string storedProcedure,
            DynamicParameters? parameters = null);

        Task<T> ExecuteScalarAsync<T>(
            string storedProcedure,
            DynamicParameters? parameters = null);
    }
}
