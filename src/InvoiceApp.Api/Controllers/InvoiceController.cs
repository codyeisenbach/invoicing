using Microsoft.AspNetCore.Mvc;
using InvoiceApp.Api.Contracts;
using InvoiceApp.Models;
using InvoiceApp.Services;

namespace InvoiceApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
    {
        return await _invoiceService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Invoice>> GetInvoiceById(Guid id)
    {
        var invoice = await _invoiceService.GetByIdAsync(id);

        if (invoice == null)
        {
            return NotFound();
        }
        return invoice;
    }

    [HttpPost]
    public async Task<ActionResult<Invoice>> Create(CreateInvoiceRequest request)
    {
        var invoice = await _invoiceService.CreateAsync(request);

        return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.InvoiceId }, invoice);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateInvoiceRequest request)
    {
        var updated = await _invoiceService.UpdateAsync(id, request);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _invoiceService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
