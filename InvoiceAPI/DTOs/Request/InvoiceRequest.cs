namespace InvoiceAPI.DTOs.Request
{
    public class InvoiceRequest
    {
        public List<InvoiceItemRequest> Items { get; set; } = new();
    }
}
