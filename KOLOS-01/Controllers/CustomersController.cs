using KOLOS_01.DTOs;
using KOLOS_01.Services;
using Microsoft.AspNetCore.Mvc;

namespace KOLOS_01.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("{id:int}/purchases")]
    public async Task<IActionResult> GetCustomerPurchases([FromRoute] int id)
    {
        var customer = await _customerService.GetCustomerPurchasesAsync(id);
        if (customer is null)
        {
            return NotFound($"Klient o ID {id} nie istnieje.");
        }

        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> AddCustomer([FromBody] CreateCustomerRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _customerService.AddCustomerWithPurchasesAsync(request);

        return result.Status switch
        {
            AddCustomerStatus.CustomerAlreadyExists => Conflict(result.Message),
            AddCustomerStatus.ConcertNotFound => NotFound(result.Message),
            AddCustomerStatus.TooManyTicketsForConcert => BadRequest(result.Message),
            AddCustomerStatus.Created =>
                Created($"/api/customers/{result.CustomerId}/purchases", new { customerId = result.CustomerId }),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
