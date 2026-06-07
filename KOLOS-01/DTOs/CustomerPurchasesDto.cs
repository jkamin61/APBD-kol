namespace KOLOS_01.DTOs;

public class CustomerPurchasesDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public List<PurchaseDto> Purchases { get; set; } = new();
}

public class PurchaseDto
{
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public TicketInfoDto Ticket { get; set; } = null!;
    public ConcertInfoDto Concert { get; set; } = null!;
}

public class TicketInfoDto
{
    public string Serial { get; set; } = null!;
    public int SeatNumber { get; set; }
}

public class ConcertInfoDto
{
    public string Name { get; set; } = null!;
    public DateTime Date { get; set; }
}
