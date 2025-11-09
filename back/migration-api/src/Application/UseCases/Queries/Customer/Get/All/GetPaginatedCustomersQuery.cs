using Application.Common.Dto;
using Application.UseCases.Commands.Seeding.Dtos;
using MediatR;

namespace Application.UseCases.Queries.Customer.Get.All;

public record GetPaginatedCustomersQuery : IRequest<PaginatedResult<CustomerDto>>
{
    public required int Page { get; set; }
    public required int Take { get; set; }
}
    