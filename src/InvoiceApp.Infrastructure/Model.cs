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

    protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptions)
    {
        if (!dbContextOptions.IsConfigured)
        {
            throw new InvalidOperationException("The DbContext is not configured. Please configure it in the startup project or use IDesignTimeDbContextFactory.");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.Property(i => i.Subtotal).HasPrecision(18, 2);
            entity.Property(i => i.Tax).HasPrecision(18, 2);
            entity.Property(i => i.TotalAmount).HasPrecision(18, 2);
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.Property(i => i.UnitPrice).HasPrecision(18, 2);
            entity.Property(i => i.LineTotal).HasPrecision(18, 2);
        });
    }

}
