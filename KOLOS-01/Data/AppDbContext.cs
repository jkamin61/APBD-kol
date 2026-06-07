using KOLOS_01.Entities;
using Microsoft.EntityFrameworkCore;

namespace KOLOS_01.Data;

public class AppDbContext : DbContext
{
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketConcert> TicketConcerts { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Concert> Concerts { get; set; }
    public DbSet<PurchasedTicket> PurchasedTickets { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TicketConcert>()
            .HasOne(tc => tc.Ticket)
            .WithMany(t => t.TicketConcerts)
            .HasForeignKey(tc => tc.TicketId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TicketConcert>()
            .HasOne(tc => tc.Concert)
            .WithMany(c => c.TicketConcerts)
            .HasForeignKey(tc => tc.ConcertId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchasedTicket>()
            .HasOne(pt => pt.TicketConcert)
            .WithMany(tc => tc.PurchasedTickets)
            .HasForeignKey(pt => pt.TicketConcertId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchasedTicket>()
            .HasOne(pt => pt.Customer)
            .WithMany(c => c.PurchasedTickets)
            .HasForeignKey(pt => pt.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
