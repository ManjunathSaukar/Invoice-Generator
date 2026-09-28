namespace InvoiceAPI.DTOs.Response
{
    public class InvoiceInsertResult
    {
        public int InvoiceId { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;
    }
}
