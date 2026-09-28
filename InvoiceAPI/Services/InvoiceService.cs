using InvoiceAPI.DTOs.Request;
using InvoiceAPI.DTOs.Response;
using InvoiceAPI.Entities;
using InvoiceAPI.Interfaces;

namespace InvoiceAPI.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IProductRepository _productRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(
            IProductRepository productRepository,
            IInvoiceRepository invoiceRepository)
        {
            _productRepository = productRepository;
            _invoiceRepository = invoiceRepository;
        }

        // Methods...

        public async Task<InvoiceResponse> GenerateInvoiceAsync(InvoiceRequest request)
        {
            ValidateRequest(request);

            var items = await BuildInvoiceItemsAsync(request);

            var invoice = CalculateInvoiceTotals(items);

            var invoiceResult = await _invoiceRepository.InsertInvoiceAsync(invoice);

            foreach (var item in items)
            {
                item.InvoiceId = invoiceResult.InvoiceId;

                await _invoiceRepository.InsertInvoiceItemAsync(item);
            }

            return BuildResponse(invoice, items, invoiceResult);
        }

        public async Task<InvoiceResponse?> GetInvoiceAsync(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(invoiceId);

            if (invoice == null)
                return null;

            var items = (await _invoiceRepository
                .GetInvoiceItemsByInvoiceIdAsync(invoiceId))
                .ToList();

            return new InvoiceResponse
            {
                InvoiceId = invoice.InvoiceId,
                InvoiceNumber = invoice.InvoiceNumber,
                InvoiceDate = invoice.InvoiceDate,
                SubTotal = invoice.SubTotal,
                VatAmount = invoice.VatAmount,
                AdditionalTax = invoice.AdditionalTax,
                GrandTotal = invoice.GrandTotal,

                Items = MapInvoiceItems(items)
            };
        }

        public async Task<InvoiceResponse?> GetInvoiceByNumberAsync(string invoiceNumber)
        {
            var invoice = await _invoiceRepository.GetInvoiceByNumberAsync(invoiceNumber);

            if (invoice == null)
                return null;

            var items = await _invoiceRepository.GetInvoiceItemsByInvoiceIdAsync(invoice.InvoiceId);

            return new InvoiceResponse
            {
                InvoiceId = invoice.InvoiceId,
                InvoiceNumber = invoice.InvoiceNumber,
                InvoiceDate = invoice.InvoiceDate,
                SubTotal = invoice.SubTotal,
                VatAmount = invoice.VatAmount,
                AdditionalTax = invoice.AdditionalTax,
                GrandTotal = invoice.GrandTotal,

                Items = items.Select(item => new InvoiceItemResponse
                {
                    InvoiceId = item.InvoiceItemId,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    LineAmount = item.LineAmount,
                    AdditionalTax = item.AdditionalTax
                }).ToList()
            };
        }


        private static void ValidateRequest(InvoiceRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Items == null || !request.Items.Any())
                throw new ArgumentException("Invoice must contain at least one item.");

            if (request.Items.Any(i => i.Quantity <= 0))
                throw new ArgumentException("Quantity should be greater than zero.");
        }
        private async Task<List<InvoiceItem>> BuildInvoiceItemsAsync(InvoiceRequest request)
        {
            var invoiceItems = new List<InvoiceItem>();

            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);

                if (product == null)
                {
                    throw new Exception($"Product with Id {item.ProductId} not found.");
                }

                decimal lineAmount = product.Price * item.Quantity;

                decimal additionalTax = product.IsImported
                    ? lineAmount * 0.05m
                    : 0;

                invoiceItems.Add(new InvoiceItem
                {
                    ProductId = product.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    LineAmount = lineAmount,
                    AdditionalTax = additionalTax
                });
            }

            return invoiceItems;
        }

        private static InvoiceHeader CalculateInvoiceTotals(List<InvoiceItem> items)
        {
            decimal subTotal = items.Sum(x => x.LineAmount);

            decimal vatAmount = subTotal * 0.12m;

            decimal additionalTax = items.Sum(x => x.AdditionalTax);

            decimal grandTotal =
                subTotal +
                vatAmount +
                additionalTax;

            return new InvoiceHeader
            {
                InvoiceDate = DateTime.Now,

                SubTotal = subTotal,

                VatAmount = vatAmount,

                AdditionalTax = additionalTax,

                GrandTotal = grandTotal
            };
        }
        private static InvoiceResponse BuildResponse(InvoiceHeader invoice, List<InvoiceItem> items, InvoiceInsertResponse invoiceResult)
        {
            return new InvoiceResponse
            {
                InvoiceNumber = invoiceResult.InvoiceNumber,

                InvoiceDate = invoice.InvoiceDate,

                SubTotal = invoice.SubTotal,

                VatAmount = invoice.VatAmount,

                AdditionalTax = invoice.AdditionalTax,

                GrandTotal = invoice.GrandTotal,

                Items = MapInvoiceItems(items)
            };
        }
        private static List<InvoiceItemResponse> MapInvoiceItems(IEnumerable<InvoiceItem> items)
        {
            return items.Select(item => new InvoiceItemResponse
            {
                InvoiceId = item.InvoiceItemId,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                AdditionalTax = item.AdditionalTax,
                LineAmount = item.LineAmount
            }).ToList();
        }
    }
}
