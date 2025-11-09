using Application.Common.Dto;
using MediatR;

namespace Application.UseCases.Commands.Seeding;

public class StartSeedingCommand : IRequest<Result<string>>
{
}
