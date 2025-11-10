using Application.Dependencies.DataAccess;
using Application.Dependencies.Logging;
using Application.Dependencies.Services;
using Application.UseCases.Commands.Seeding.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.ApplicationDependencies.Services;

public sealed class CustomerBatchProcessor : ICustomerBatchProcessor
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public CustomerBatchProcessor(IUnitOfWork unitOfWork, IMapper mapper, ILoggerManager logger)
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

        var entities = await GetEntityToCreate(batchDtos);
        if(!entities.Any())
        {
            _logger.LogInfo($"Записи из батча в БД уже существуют, их CardCod`ы: {string.Join(", ", batchDtos.Select(dto => dto.CardCode))}.");
            return 0;
        }

        token.ThrowIfCancellationRequested();
        try
        {
            // TODO: Cтоит вынести транзакции на уровень выше.
            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.Customers.AddRange(entities);
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw new Exception($"Произошла ошибка при сохранении данных в БД: {ex}.");
        }

        return entities.Count;
    }

    /// <summary>
    /// Найти записи, CardCode которых еще не сохранен в БД.
    /// Комбинируем CardCode и PhoneMobile, чтобы найти задублированные записи, тк CardCode не уникален.
    /// </summary>
    /// <param name="batchDtos"></param>
    /// <returns></returns>
    private async Task<List<CustomerEntity>> GetEntityToCreate(List<CustomerDto> batchDtos)
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

        return _mapper.Map<List<CustomerEntity>>(dtoToCreateEntities);
    }
}
