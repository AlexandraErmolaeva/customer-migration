using Application.Common.Dto;
using Application.Dependencies.Logging;
using Application.UseCases.Commands.Seeding;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerMigrationApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedingController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILoggerManager _logger;

    public SeedingController(IMediator mediator, ILoggerManager logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("Start")]
    public async Task<ActionResult<Result<string>>> StartSeedingAsync([FromBody] StartSeedingCommand command)
    {
        var result = await _mediator.Send(command);
        return result;
    }
}