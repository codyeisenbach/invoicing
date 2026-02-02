namespace InvoiceApp.Models;

public class Invoice
{
    public Guid InvoiceId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public Guid CustomerId { get; set; }
    public required string CustomerName { get; set; }
    public required List<InvoiceItem> InvoiceItems { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public DateTime DueDate { get; set; }
    public List<Payment>? Payments { get; set; }
}