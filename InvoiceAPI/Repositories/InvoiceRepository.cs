using Dapper;
using InvoiceAPI.Constants;
using InvoiceAPI.Data;
using InvoiceAPI.DTOs.Response;
using InvoiceAPI.Entities;
using InvoiceAPI.Interfaces;

namespace InvoiceAPI.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IDapperRepository _dapperRepository;

        public InvoiceRepository(IDapperRepository dapperRepository)
        {
            _dapperRepository = dapperRepository;
        }

        // Methods...

        public async Task<InvoiceInsertResponse> InsertInvoiceAsync(InvoiceHeader invoice)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@InvoiceDate", invoice.InvoiceDate);
            parameters.Add("@SubTotal", invoice.SubTotal);
            parameters.Add("@VatAmount", invoice.VatAmount);
            parameters.Add("@AdditionalTax", invoice.AdditionalTax);
            parameters.Add("@GrandTotal", invoice.GrandTotal);

            var result = await _dapperRepository.QueryFirstOrDefaultAsync<InvoiceInsertResponse>(
                StoredProcedures.InsertInvoice,
                parameters);

            return result!;
        }

        public async Task<int> InsertInvoiceItemAsync(InvoiceItem invoiceItem)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@InvoiceId", invoiceItem.InvoiceId);
            parameters.Add("@ProductId", invoiceItem.ProductId);
            parameters.Add("@Quantity", invoiceItem.Quantity);
            parameters.Add("@UnitPrice", invoiceItem.UnitPrice);
            parameters.Add("@LineAmount", invoiceItem.LineAmount);
            parameters.Add("@AdditionalTax", invoiceItem.AdditionalTax);

            return await _dapperRepository.ExecuteAsync(
                StoredProcedures.InsertInvoiceItem,
                parameters);
        }

        public async Task<InvoiceHeader?> GetInvoiceByIdAsync(int invoiceId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@InvoiceId", invoiceId);

            return await _dapperRepository.QueryFirstOrDefaultAsync<InvoiceHeader>(
                StoredProcedures.GetInvoiceById,
                parameters);
        }

        public async Task<IEnumerable<InvoiceItem>> GetInvoiceItemsByInvoiceIdAsync(int invoiceId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@InvoiceId", invoiceId);

            return await _dapperRepository.QueryAsync<InvoiceItem>(
                StoredProcedures.GetInvoiceItemsByInvoiceId,
                parameters);
        }

        public async Task<InvoiceHeader?> GetInvoiceByNumberAsync(string invoiceNumber)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@InvoiceNumber", invoiceNumber);

            return await _dapperRepository.QueryFirstOrDefaultAsync<InvoiceHeader>(
                StoredProcedures.GetInvoiceByNumber,
                parameters);
        }
    }
}
