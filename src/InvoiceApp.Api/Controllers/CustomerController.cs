using Microsoft.AspNetCore.Mvc;
using InvoiceApp.Api.Contracts;
using InvoiceApp.Models;
using InvoiceApp.Services;

namespace InvoiceApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        return await _customerService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomerById(Guid id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer == null)
        {
            return NotFound();
        }
        return customer;
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> Create(CreateCustomerRequest request)
    {
        var customer = await _customerService.CreateAsync(request);

        return CreatedAtAction(nameof(GetCustomerById), new { id = customer.CustomerId }, customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateCustomerRequest request)
    {
        var updated = await _customerService.UpdateAsync(id, request);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _customerService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
