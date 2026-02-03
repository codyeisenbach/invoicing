using InvoiceApp.Models;
namespace InvoiceApp.Api.Contracts;

public sealed class CreateCustomerRequest
{
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
}
