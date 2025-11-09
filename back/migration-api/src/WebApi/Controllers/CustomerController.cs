using Application.Common.Dto;
using Application.UseCases.Commands.Seeding.Dtos;
using Application.UseCases.Commands.Update.Customer;
using Application.UseCases.Queries.Customer.Get.All;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Get/All")]
    public async Task<ActionResult<PaginatedResult<CustomerDto>>> StartSeedingAsync([FromQuery] GetPaginatedCustomersQuery query)
    {
        var result = await _mediator.Send(query);
        return result;
    }

    [HttpPost("Update")]
    public async Task<ActionResult<Result<CustomerDto>>> UpdateCustomer([FromBody] UpdateCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return result;
    }
}
