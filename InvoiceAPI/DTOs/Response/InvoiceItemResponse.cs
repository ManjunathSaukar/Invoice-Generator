namespace InvoiceAPI.DTOs.Response
{
    public class InvoiceItemResponse
    {
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal AdditionalTax { get; set; }

        public decimal LineAmount { get; set; }
    }
}
