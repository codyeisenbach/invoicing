namespace InvoiceApp.Api.Contracts;

public sealed class CreateInvoiceItemRequest
{
    public decimal UnitPrice { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
}
