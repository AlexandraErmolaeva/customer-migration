using Application.Common.Dto;
using Application.Dependencies.Logging;
using Application.Dependencies.Services;
using MediatR;
using System.Diagnostics;

namespace Application.UseCases.Commands.Seeding;

public class StartSeedingCommandHandler : IRequestHandler<StartSeedingCommand, Result<string>>
{
    private readonly IExcelCustomersReader _reader;
    private readonly ICustomerBatchProcessor _batchProcesser;
    private readonly ILoggerManager _logger;

    private const int BATCH_SIZE = 30;
    private const string FILE_NAME = "testCards.xlsx";
    private const string FILE_PATH = "data";

    public StartSeedingCommandHandler(IExcelCustomersReader reader, ICustomerBatchProcessor batchProcesser, ILoggerManager logger)
    {
        _reader = reader;
        _batchProcesser = batchProcesser;
        _logger = logger;
    }

    /// <summary>
    /// Мигрируем данные из екселя в БД батчами.
    /// Библиотеку выбрала ExcelDataReader, тк позволяет потоком забирать батчи, а не грузить весь ексель файл в память.
    /// </summary>
    public async Task<Result<string>> Handle(StartSeedingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), FILE_PATH, FILE_NAME);
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл не найден по пути: {filePath}.");

            // --
            var sw = new Stopwatch();
            sw.Start();
            // --

            var totalProcessed = 0;
            await foreach (var batch in _reader.ReadCustomersDataAsync(filePath, BATCH_SIZE, cancellationToken))
            {
                var processedCount = await _batchProcesser.ProcessCustomerBatchAsync(batch, cancellationToken);
                totalProcessed += processedCount;
                // await Task.Delay(666); Убираем, чтоб не ждать.
            }

            // --
            sw.Stop();
            // --

            if (totalProcessed == 0)
                return Result<string>.Success($"Все записи о клиентах уже были перенесены в БД! Время перепроверки составило {sw.ElapsedMilliseconds}ms");

            return Result<string>.Success($"Записи о клиентах в количестве {totalProcessed} успешно обработаны и сохранены в БД! Время выполнение операции {sw.ElapsedMilliseconds}ms.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Произошла ошибка при миграции данных {ex}.");
            return Result<string>.Failure(ex.Message);
        }
    }
}
