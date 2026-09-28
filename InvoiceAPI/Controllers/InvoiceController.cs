using InvoiceAPI.DTOs.Request;
using InvoiceAPI.DTOs.Response;
using InvoiceAPI.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : BaseController
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateInvoice([FromBody] InvoiceRequest request)
        {
            var response = await _invoiceService.GenerateInvoiceAsync(request);

            return Success(response, "Invoice generated successfully.");
        }

        [HttpGet("id/{invoiceId:int}")]
        public async Task<IActionResult> GetInvoice(int invoiceId)
        {
            var response = await _invoiceService.GetInvoiceAsync(invoiceId);

            if (response == null)
                return NotFoundResponse("Invoice not found.");

            return Success(response, "Invoice retrieved successfully.");
        }

        [HttpGet("{invoiceNumber}")]
        public async Task<IActionResult> GetInvoiceByNumber(string invoiceNumber)
        {
            var invoice = await _invoiceService.GetInvoiceByNumberAsync(invoiceNumber);

            if (invoice == null)
                return NotFoundResponse("Invoice not found.");

            return Success(invoice, "Invoice retrieved successfully.");
        }
    }
}