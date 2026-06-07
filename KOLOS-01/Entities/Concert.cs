using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KOLOS_01.Entities;

[Table("Concert")]
public class Concert
{
    [Key]
    public int ConcertId { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    public int AvailableTickets { get; set; }

    public ICollection<TicketConcert> TicketConcerts { get; set; } = new List<TicketConcert>();
}
