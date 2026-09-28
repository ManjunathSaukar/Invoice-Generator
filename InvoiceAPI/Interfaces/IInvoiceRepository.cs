using InvoiceAPI.DTOs.Response;
using InvoiceAPI.Entities;

namespace InvoiceAPI.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<InvoiceInsertResponse> InsertInvoiceAsync(InvoiceHeader invoice);

        Task<int> InsertInvoiceItemAsync(InvoiceItem invoiceItem);

        Task<InvoiceHeader?> GetInvoiceByIdAsync(int invoiceId);

        Task<IEnumerable<InvoiceItem>> GetInvoiceItemsByInvoiceIdAsync(int invoiceId);
        Task<InvoiceHeader?> GetInvoiceByNumberAsync(string invoiceNumber);
    }
}
