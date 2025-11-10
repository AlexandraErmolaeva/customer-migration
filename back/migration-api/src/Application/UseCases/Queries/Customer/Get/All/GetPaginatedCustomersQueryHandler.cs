using Application.Common.Dto;
using Application.Common.Specifications;
using Application.Dependencies.DataAccess;
using Application.UseCases.Commands.Seeding.Dtos;
using MediatR;

namespace Application.UseCases.Queries.Customer.Get.All;

public class GetPaginatedCustomersQueryHandler : IRequestHandler<GetPaginatedCustomersQuery, PaginatedResult<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedCustomersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Забираем пагинированные записи из БД.
    /// </summary>
    public async Task<PaginatedResult<CustomerDto>> Handle(GetPaginatedCustomersQuery request, CancellationToken cancellationToken)
    {
        var spec = new CustomerSpecification();
        var paginatedResult = await _unitOfWork.Customers.GetPaginatedProjectionAsync<CustomerDto>(spec, request.Take, request.Page);
        var customerDtos = paginatedResult.Customers;
        var totalCount = paginatedResult.Count;

        return new() { PageItems = customerDtos, TotalCount = totalCount };
    }
}
