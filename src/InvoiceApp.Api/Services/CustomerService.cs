using InvoiceApp.Api.Contracts;
using InvoiceApp.Infrastructure;
using InvoiceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Services;

public interface ICustomerService
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(Guid id);
    Task<Customer> CreateAsync(CreateCustomerRequest request);
    Task<bool> UpdateAsync(Guid id, UpdateCustomerRequest request);
    Task<bool> DeleteAsync(Guid id);
}

public sealed class CustomerService : ICustomerService
{
    private readonly InvoiceAppContext _context;

    public CustomerService(InvoiceAppContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .Include(i => i.Invoices)
            .ThenInclude(i => i.InvoiceItems)
            .ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await _context.Customers
            .Include(c => c.Invoices)
            .ThenInclude(i => i.InvoiceItems)
            .FirstOrDefaultAsync(c => c.CustomerId == id);
    }

    public async Task<Customer> CreateAsync(CreateCustomerRequest request)
    {
        var customer = new Customer
        {
            CustomerId = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            City = request.City,
            State = request.State,
            ZipCode = request.ZipCode,
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateCustomerRequest request)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(i => i.CustomerId == id);

        if (customer == null)
        {
            return false;
        }

        customer.Name = request.Name;
        customer.Email = request.Email;
        customer.Phone = request.Phone;
        customer.Address = request.Address;
        customer.City = request.City;
        customer.State = request.State;
        customer.ZipCode = request.ZipCode;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            return false;
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return true;
    }
}
