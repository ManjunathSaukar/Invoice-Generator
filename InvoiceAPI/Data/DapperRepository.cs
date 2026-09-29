using Dapper;
using System.Data;

namespace InvoiceAPI.Data
{
    public class DapperRepository : IDapperRepository
    {
        private readonly DapperContext _dbcontext;

        public DapperRepository(DapperContext dapperContext)
        {
            _dbcontext = dapperContext;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, DynamicParameters? parameters = null)
        {
            using var connection = _dbcontext.CreateConnection();

            return await connection.QueryAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string storedProcedure, DynamicParameters? parameters = null)
        {
            using var connection = _dbcontext.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ExecuteAsync(string storedProcedure, DynamicParameters? parameters = null)
        {
            using var connection = _dbcontext.CreateConnection();

            return await connection.ExecuteAsync(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<T> ExecuteScalarAsync<T>(string storedProcedure, DynamicParameters? parameters = null)
        {
            using var connection = _dbcontext.CreateConnection();

            return await connection.ExecuteScalarAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}
