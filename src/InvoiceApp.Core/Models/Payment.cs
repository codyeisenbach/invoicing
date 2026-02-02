namespace InvoiceApp.Models;

public class Payment
{
    public Guid PaymentId { get; set; }
    public int Amount { get; set; }
    public DateTime PaymentDate { get; set; }
}