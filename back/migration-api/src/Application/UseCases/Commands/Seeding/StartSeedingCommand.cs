using Application.Common.Dto;
using MediatR;

namespace Application.UseCases.Commands.Seeding;

public record StartSeedingCommand : IRequest<Result<string>>
{
}
