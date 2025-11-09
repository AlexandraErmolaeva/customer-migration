using Application.UseCases.Commands.Seeding.Dtos;

namespace Application.Dependencies.Services;

public interface IExcelCustomersReader
{
    IAsyncEnumerable<List<CustomerDto>> ReadCustomersDataAsync(string filePath, int batchSize, CancellationToken token = default);
}
