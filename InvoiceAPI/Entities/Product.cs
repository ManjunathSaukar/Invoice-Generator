namespace InvoiceAPI.Entities
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool IsImported { get; set; }

        public bool IsActive { get; set; }
    }
}
