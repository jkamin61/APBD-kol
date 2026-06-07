using KOLOS_01.Data;
using KOLOS_01.DTOs;
using KOLOS_01.Entities;
using Microsoft.EntityFrameworkCore;

namespace KOLOS_01.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerPurchasesDto?> GetCustomerPurchasesAsync(int customerId)
    {
        return await _context.Customers
            .Where(c => c.CustomerId == customerId)
            .Select(c => new CustomerPurchasesDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber,
                Purchases = c.PurchasedTickets
                    .Select(pt => new PurchaseDto
                    {
                        Date = pt.PurchaseDate,
                        Price = pt.TicketConcert.Price,
                        Ticket = new TicketInfoDto
                        {
                            Serial = pt.TicketConcert.Ticket.SerialNumber,
                            SeatNumber = pt.TicketConcert.Ticket.SeatNumber
                        },
                        Concert = new ConcertInfoDto
                        {
                            Name = pt.TicketConcert.Concert.Name,
                            Date = pt.TicketConcert.Concert.Date
                        }
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<AddCustomerResult> AddCustomerWithPurchasesAsync(CreateCustomerRequestDto request)
    {
        var customerExists = await _context.Customers
            .AnyAsync(c => c.CustomerId == request.Customer.Id);
        if (customerExists)
        {
            return new AddCustomerResult(AddCustomerStatus.CustomerAlreadyExists,
                Message: $"Klient o ID {request.Customer.Id} juz istnieje.");
        }

        var concertNames = request.Purchases.Select(p => p.ConcertName).Distinct().ToList();
        var concerts = await _context.Concerts
            .Where(c => concertNames.Contains(c.Name))
            .ToListAsync();

        var missingConcert = concertNames.FirstOrDefault(n => concerts.All(c => c.Name != n));
        if (missingConcert is not null)
        {
            return new AddCustomerResult(AddCustomerStatus.ConcertNotFound,
                Message: $"Koncert o nazwie \"{missingConcert}\" nie istnieje.");
        }

        var overLimit = request.Purchases
            .GroupBy(p => p.ConcertName)
            .FirstOrDefault(g => g.Count() > 5);
        if (overLimit is not null)
        {
            return new AddCustomerResult(AddCustomerStatus.TooManyTicketsForConcert,
                Message: $"Nie mozna kupic wiecej niz 5 biletow na koncert \"{overLimit.Key}\".");
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        var customer = new Customer
        {
            FirstName = request.Customer.FirstName,
            LastName = request.Customer.LastName,
            PhoneNumber = request.Customer.PhoneNumber
        };
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        foreach (var purchase in request.Purchases)
        {
            var concert = concerts.First(c => c.Name == purchase.ConcertName);

            var ticket = new Ticket
            {
                SerialNumber = GenerateSerialNumber(),
                SeatNumber = purchase.SeatNumber
            };
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            var ticketConcert = new TicketConcert
            {
                TicketId = ticket.TicketId,
                ConcertId = concert.ConcertId,
                Price = purchase.Price
            };
            await _context.TicketConcerts.AddAsync(ticketConcert);
            await _context.SaveChangesAsync();

            var purchasedTicket = new PurchasedTicket
            {
                TicketConcertId = ticketConcert.TicketConcertId,
                CustomerId = customer.CustomerId,
                PurchaseDate = DateTime.Now
            };
            await _context.PurchasedTickets.AddAsync(purchasedTicket);
            await _context.SaveChangesAsync();
        }

        await transaction.CommitAsync();

        return new AddCustomerResult(AddCustomerStatus.Created, customer.CustomerId);
    }

    private static string GenerateSerialNumber()
    {
        return $"TK{Guid.NewGuid():N}"[..15].ToUpper();
    }
}
