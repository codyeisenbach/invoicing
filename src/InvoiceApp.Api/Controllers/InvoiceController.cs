using Microsoft.AspNetCore.Mvc;
using InvoiceApp.Models;
using InvoiceApp.Api.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InvoiceApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly InvoiceAppContext _context;

    public InvoiceController(InvoiceAppContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
    {
        // Use .Include() to tell EF Core to join the InvoiceItems table
        return await _context.Invoices
            .Include(i => i.InvoiceItems)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Invoice>> GetInvoiceById(Guid id)
    {
        // FindAsync doesn't support Include, so we use FirstOrDefaultAsync
        var invoice = await _context.Invoices
            .Include(i => i.InvoiceItems)
            .FirstOrDefaultAsync(i => i.InvoiceId == id);

        if (invoice == null)
        {
            return NotFound();
        }
        return invoice;
    }

    [HttpPost]
    public async Task<ActionResult<Invoice>> Create(CreateInvoiceRequest request)
    {
        var invoice = new Invoice
        {
            CustomerId = request.CustomerId,
            CustomerName = request.CustomerName,
            InvoiceDate = request.InvoiceDate,
            DueDate = request.DueDate,
            InvoiceItems = request.Items
                .Select(item => new InvoiceItem
                {
                    UnitPrice = item.UnitPrice,
                    Description = item.Description,
                    Quantity = item.Quantity
                })
                .ToList()
        };

        foreach (var item in invoice.InvoiceItems)
        {
            item.RecalculateLineTotal();
        }

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.InvoiceId }, invoice);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateInvoiceRequest request)
    {
        var invoice = await _context.Invoices
            .Include(i => i.InvoiceItems)
            .FirstOrDefaultAsync(i => i.InvoiceId == id);

        if (invoice == null)
        {
            return NotFound();
        }

        invoice.CustomerId = request.CustomerId;
        invoice.CustomerName = request.CustomerName;
        invoice.InvoiceDate = request.InvoiceDate;
        invoice.DueDate = request.DueDate;

        if (invoice.InvoiceItems.Count > 0)
        {
            _context.InvoiceItems.RemoveRange(invoice.InvoiceItems);
        }

        invoice.InvoiceItems = request.Items
            .Select(item => new InvoiceItem
            {
                UnitPrice = item.UnitPrice,
                Description = item.Description,
                Quantity = item.Quantity
            })
            .ToList();

        foreach (var item in invoice.InvoiceItems)
        {
            item.RecalculateLineTotal();
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
