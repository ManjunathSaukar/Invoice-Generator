using Dapper;
using InvoiceAPI.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InvoiceAPI.Repositories
{
    public class DapperRepository : IDapperRepository
    {
        private readonly IConfiguration _configuration;

        public DapperRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, DynamicParameters? parameters = null)
        {
            using var connection = CreateConnection();

            return await connection.QueryAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string storedProcedure, DynamicParameters? parameters = null)
        {
            using var connection = CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ExecuteAsync(string storedProcedure, DynamicParameters? parameters = null)
        {
            using var connection = CreateConnection();

            return await connection.ExecuteAsync(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<T> ExecuteScalarAsync<T>(string storedProcedure, DynamicParameters? parameters = null)
        {
            using var connection = CreateConnection();

            return await connection.ExecuteScalarAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}
