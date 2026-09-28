namespace InvoiceAPI.DTOs.Response
{
    public class InvoiceResponse
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; }

        public decimal SubTotal { get; set; }

        public decimal VatAmount { get; set; }

        public decimal AdditionalTax { get; set; }

        public decimal GrandTotal { get; set; }

        public List<InvoiceItemResponse> Items { get; set; } = new();
    }
}
