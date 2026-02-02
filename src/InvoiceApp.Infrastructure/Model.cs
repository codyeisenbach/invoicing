using InvoiceApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class InvoiceAppContext : DbContext
{
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }

    public InvoiceAppContext(DbContextOptions<InvoiceAppContext> options)
        : base(options)
    {
    }

    public InvoiceAppContext()
    {
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptions)
    {
        if (!dbContextOptions.IsConfigured)
        {
            throw new InvalidOperationException("The DbContext is not configured. Please configure it in the startup project or use IDesignTimeDbContextFactory.");
        }
    }

}
