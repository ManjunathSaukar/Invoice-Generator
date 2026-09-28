namespace InvoiceAPI.Constants
{
    public static class StoredProcedures
    {
        // Product
        public const string GetProducts = "sp_GetProducts";
        public const string GetProductById = "sp_GetProductById";

        // Invoice
        public const string InsertInvoice = "sp_InsertInvoice";
        public const string InsertInvoiceItem = "sp_InsertInvoiceItem";
        public const string GetInvoiceById = "sp_GetInvoiceById";
        public const string GetInvoiceItemsByInvoiceId = "sp_GetInvoiceItemsByInvoiceId";
        public const string GetInvoiceByNumber = "sp_GetInvoiceByNumber";
    }
}