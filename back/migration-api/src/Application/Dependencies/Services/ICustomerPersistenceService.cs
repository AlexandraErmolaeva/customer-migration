using Application.UseCases.Commands.Seeding.Dtos;

namespace Application.Dependencies.Services;

public interface ICustomerPersistenceService
{
    Task<int> ProcessCustomerBatchAsync(List<CustomerDto> batchDtos, CancellationToken token = default);
}
