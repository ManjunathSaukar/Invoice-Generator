namespace InvoiceAPI.DTOs.Response
{
    public class InvoiceInsertResponse
    {
        public int InvoiceId { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;
    }
}
