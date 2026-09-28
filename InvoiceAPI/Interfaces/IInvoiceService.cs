using InvoiceAPI.DTOs.Request;
using InvoiceAPI.DTOs.Response;

namespace InvoiceAPI.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceResponse> GenerateInvoiceAsync(InvoiceRequest request);

        Task<InvoiceResponse?> GetInvoiceAsync(int invoiceId);
        Task<InvoiceResponse?> GetInvoiceByNumberAsync(string invoiceNumber);
    }
}
