using KOLOS_01.DTOs;

namespace KOLOS_01.Services;

public enum AddCustomerStatus
{
    Created,
    CustomerAlreadyExists,
    ConcertNotFound,
    TooManyTicketsForConcert
}

public record AddCustomerResult(AddCustomerStatus Status, int? CustomerId = null, string? Message = null);

public interface ICustomerService
{
    Task<CustomerPurchasesDto?> GetCustomerPurchasesAsync(int customerId);
    Task<AddCustomerResult> AddCustomerWithPurchasesAsync(CreateCustomerRequestDto request);
}
