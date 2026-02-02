namespace InvoiceApp.Api.Contracts;

public sealed class UpdateInvoiceRequest
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = "";
    public List<UpdateInvoiceItemRequest> Items { get; set; } = new();
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
}
