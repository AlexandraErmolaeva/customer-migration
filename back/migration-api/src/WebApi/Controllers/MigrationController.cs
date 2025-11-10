using Application.Common.Dto;
using Application.UseCases.Commands.Seeding;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CustomerMigrationApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MigrationController : ControllerBase
{
    private readonly IMediator _mediator;

    public MigrationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [SwaggerOperation(Summary = "Начать миграцию записей из xlsx файла в БД.")]
    [HttpPost("Start")]
    public async Task<ActionResult<Result<string>>> StartMigration()
    {
        var result = await _mediator.Send(new StartSeedingCommand());
        return result;
    }
}