namespace InvoiceApp.Api.Contracts;

public sealed class CreateInvoiceRequest
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = "";
    public List<CreateInvoiceItemRequest> Items { get; set; } = new();
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
}
