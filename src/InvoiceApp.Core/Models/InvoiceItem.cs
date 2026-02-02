namespace InvoiceApp.Models;

public class InvoiceItem
{
    public Guid InvoiceItemId { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal { get; private set; }

    public void RecalculateLineTotal()
    {
        LineTotal = UnitPrice * Quantity;
    }
}
