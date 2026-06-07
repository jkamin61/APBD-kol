using System.ComponentModel.DataAnnotations;

namespace KOLOS_01.DTOs;

public class CreateCustomerRequestDto
{
    [Required]
    public CustomerDataDto Customer { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public List<PurchaseRequestDto> Purchases { get; set; } = new();
}

public class CustomerDataDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [MaxLength(100)]
    public string? PhoneNumber { get; set; }
}

public class PurchaseRequestDto
{
    [Required]
    public int SeatNumber { get; set; }

    [Required]
    [MaxLength(100)]
    public string ConcertName { get; set; } = null!;

    [Range(0.01, double.MaxValue, ErrorMessage = "Cena musi byc wieksza od zera.")]
    public decimal Price { get; set; }
}
