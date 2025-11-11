using Application.Dependencies.DataAccess;
using Application.Dependencies.Logging;
using Application.Dependencies.Services;
using Application.UseCases.Commands.Seeding.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.ApplicationDependencies.Services;

public sealed class CustomerPersistenceService : ICustomerPersistenceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public CustomerPersistenceService(IUnitOfWork unitOfWork, IMapper mapper, ILoggerManager logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Сохранить батч с записями дто в БД.
    /// </summary>
    /// <param name="batchDtos"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<int> ProcessCustomerBatchAsync(List<CustomerDto> batchDtos, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var (entitiesToCreate, entitiesToUpdate) = await GetEntitiesToProcess(batchDtos);
        if (entitiesToCreate.Count + entitiesToUpdate.Count == 0)
        {
            _logger.LogInfo($"Записи из батча в БД уже существуют, их CardCod`ы: {string.Join(", ", batchDtos.Select(dto => dto.CardCode))}.");
            return 0;
        }

        token.ThrowIfCancellationRequested();
        try
        {
            // TODO: Вынести транзакцию на уровень выше.
            await _unitOfWork.BeginTransactionAsync();

            if (entitiesToCreate.Any())
                _unitOfWork.Customers.AddRange(entitiesToCreate);

            if (entitiesToUpdate.Any())
                _unitOfWork.Customers.UpdateRange(entitiesToUpdate);

            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw new Exception("Произошла ошибка при сохранении данных в БД.", ex);
        }

        _logger.LogInfo($"Добавили записей: {entitiesToCreate.Count} | обновили записей: {entitiesToUpdate.Count}");
        return entitiesToCreate.Count + entitiesToUpdate.Count;
    }

    /// <summary>
    /// Найти записи, CardCode и PhoneMobile которых еще не сохранен в БД. Формируем записи для сохранения или обновления.
    /// Комбинируем CardCode и PhoneMobile, чтобы найти задублированные записи, тк CardCode не уникален.
    /// </summary>
    /// <param name="batchDtos"></param>
    /// <returns></returns>
    private async Task<(List<CustomerEntity> EntityToCreate, List<CustomerEntity> EntityToUpdate)> GetEntitiesToProcess(List<CustomerDto> batchDtos)
    {
        var dtoCardCodes = batchDtos.Select(dto => dto.CardCode).ToList();

        var existingByCardCodeDtos = await _unitOfWork.Customers
            .GetProjectedListAsync<CustomerDto>(e => dtoCardCodes.Contains(e.CardCode));

        var existingPairs = existingByCardCodeDtos
            .Select(c => (c.CardCode, c.Contacts?.PhoneMobile))
            .ToHashSet();

        var dtoToCreateEntities = batchDtos
            .Where(dto => !existingPairs.Contains((dto.CardCode, dto.Contacts?.PhoneMobile)))
            .ToList();

        var dtoToUpdateEntities = batchDtos
            .Where(dto => existingPairs.Contains((dto.CardCode, dto.Contacts?.PhoneMobile)))
            .ToList();

        return (_mapper.Map<List<CustomerEntity>>(dtoToCreateEntities), _mapper.Map<List<CustomerEntity>>(dtoToUpdateEntities));
    }
}
