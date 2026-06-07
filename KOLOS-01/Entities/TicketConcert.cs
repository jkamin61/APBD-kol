using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KOLOS_01.Entities;

[Table("Ticket_Concert")]
public class TicketConcert
{
    [Key]
    public int TicketConcertId { get; set; }

    public int TicketId { get; set; }

    public int ConcertId { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [ForeignKey(nameof(TicketId))]
    public Ticket Ticket { get; set; } = null!;

    [ForeignKey(nameof(ConcertId))]
    public Concert Concert { get; set; } = null!;

    public ICollection<PurchasedTicket> PurchasedTickets { get; set; } = new List<PurchasedTicket>();
}
