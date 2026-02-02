using InvoiceApp.Models;

namespace InvoiceApp.Services;

public static class InvoiceService
{
    static List<Invoice> Invoices { get; }
    static int nextId = 3;
    static InvoiceService() => Invoices = new List<Invoice>
        {
            new() {
                InvoiceId = Guid.NewGuid(),
                InvoiceDate = DateTime.Now.AddDays(-5), // 5 days ago
                CustomerId = Guid.NewGuid(),
                CustomerName = "Acme Corp",
                Subtotal = 150.00m,
                Tax = 12.00m,
                TotalAmount = 162.00m,
                DueDate = DateTime.Now.AddDays(25),
                TestId = 1,
                InvoiceItems = new List<InvoiceItem>
                {
                    new InvoiceItem
                    {
                        InvoiceItemId = Guid.NewGuid(),
                        Description = "Widget Type A",
                        Quantity = 5,
                        UnitPrice = 20.00m,
                        LineTotal = 100.00m
                    },
                    new InvoiceItem
                    {
                        InvoiceItemId = Guid.NewGuid(),
                        Description = "Service Fee",
                        Quantity = 1,
                        UnitPrice = 50.00m,
                        LineTotal = 50.00m
                    }
                }
            },
            new() {
                InvoiceId = Guid.NewGuid(),
                InvoiceDate = DateTime.Now.AddDays(-1),
                CustomerId = Guid.NewGuid(),
                CustomerName = "Jane Doe",
                Subtotal = 40.00m,
                Tax = 3.20m,
                TotalAmount = 43.20m,
                DueDate = DateTime.Now.AddDays(29),
                TestId = 2,
                InvoiceItems = new List<InvoiceItem>
                {
                    new InvoiceItem
                    {
                        InvoiceItemId = Guid.NewGuid(),
                        Description = "Consulting Hour",
                        Quantity = 2,
                        UnitPrice = 20.00m,
                        LineTotal = 40.00m
                    }
                }
            }
        };

    public static List<Invoice> GetAll() => Invoices;
    public static Invoice? Get(int id) => Invoices.FirstOrDefault(invoice => invoice.TestId == id);
    public static void Add(Invoice invoice)
    {
        invoice.TestId = nextId++;
        Invoices.Add(invoice);
    }

    public static void Delete(int id)
    {
        var invoice = Get(id);
        if (invoice is null)
            return;

        Invoices.Remove(invoice);
    }

    public static void Update(Invoice invoice)
    {
        var index = Invoices.FindIndex(i => i.TestId == invoice.TestId);
        if (index == -1)
            return;

        Invoices[index] = invoice;
    }

}