using Application.UseCases.Commands.Seeding.Dtos;

namespace Application.Dependencies.Services;

public interface ICustomerBatchProcessor
{
    Task<int> ProcessCustomerBatchAsync(List<CustomerDto> batchDto, CancellationToken token = default);
}
