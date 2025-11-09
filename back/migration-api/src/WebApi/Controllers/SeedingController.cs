using Application.Common.Dto;
using Application.UseCases.Commands.Seeding;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerMigrationApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedingController : ControllerBase
{
    private readonly IMediator _mediator;

    public SeedingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Начать миграцию записей из ексель таблицы в БД.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("Start")]
    public async Task<ActionResult<Result<string>>> StartSeedingAsync()
    {
        var result = await _mediator.Send(new StartSeedingCommand());
        return result;
    }
}