using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KOLOS_01.Entities;

[Table("Ticket")]
public class Ticket
{
    [Key]
    public int TicketId { get; set; }

    [MaxLength(50)]
    public string SerialNumber { get; set; } = null!;

    public int SeatNumber { get; set; }

    public ICollection<TicketConcert> TicketConcerts { get; set; } = new List<TicketConcert>();
}
