namespace InvoiceAPI.Entities
{
    public class InvoiceHeader
    {
        public int InvoiceId { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; }

        public decimal SubTotal { get; set; }

        public decimal VatAmount { get; set; }

        public decimal AdditionalTax { get; set; }

        public decimal GrandTotal { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
