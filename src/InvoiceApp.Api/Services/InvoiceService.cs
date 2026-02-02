using InvoiceApp.Api.Contracts;
using InvoiceApp.Infrastructure;
using InvoiceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Services;

public interface IInvoiceService
{
    Task<List<Invoice>> GetAllAsync();
    Task<Invoice?> GetByIdAsync(Guid id);
    Task<Invoice> CreateAsync(CreateInvoiceRequest request);
    Task<bool> UpdateAsync(Guid id, UpdateInvoiceRequest request);
    Task<bool> DeleteAsync(Guid id);
}

public sealed class InvoiceService : IInvoiceService
{
    private readonly InvoiceAppContext _context;

    public InvoiceService(InvoiceAppContext context)
    {
        _context = context;
    }

    public async Task<List<Invoice>> GetAllAsync()
    {
        return await _context.Invoices
            .Include(i => i.InvoiceItems)
            .ToListAsync();
    }

    public async Task<Invoice?> GetByIdAsync(Guid id)
    {
        return await _context.Invoices
            .Include(i => i.InvoiceItems)
            .FirstOrDefaultAsync(i => i.InvoiceId == id);
    }

    public async Task<Invoice> CreateAsync(CreateInvoiceRequest request)
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

        return invoice;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateInvoiceRequest request)
    {
        var invoice = await _context.Invoices
            .Include(i => i.InvoiceItems)
            .FirstOrDefaultAsync(i => i.InvoiceId == id);

        if (invoice == null)
        {
            return false;
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
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null)
        {
            return false;
        }

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();
        return true;
    }
}
