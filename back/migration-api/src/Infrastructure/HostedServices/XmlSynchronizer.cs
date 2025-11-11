using Application.Dependencies.Logging;
using Application.Dependencies.Services;
using Application.UseCases.Commands.Seeding.Dtos;
using Infrastructure.Helpers;
using Infrastructure.HostedServices.Common;
using Infrastructure.HostedServices.Config;
using Infrastructure.HostedServices.Dtos;
using Infrastructure.InternalDependencies.Serializers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infrastructure.HostedServices;

public class XmlSynchronizer : BackgroundServiceBase
{
    private readonly ILoggerManager _logger;
    private readonly IXmlSerealizer _serealizer;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly XmlSynchronizerConfig _config;

    public XmlSynchronizer(IXmlSerealizer serealizer, IServiceScopeFactory scopeFactory, IHostApplicationLifetime lifetime, ILoggerManager logger, IOptions<XmlSynchronizerConfig> config) : base(lifetime, logger, config)
    {
        _logger = logger;
        _config = config.Value;
        _serealizer = serealizer;
        _scopeFactory = scopeFactory;
    }

    public override async Task ToDo()
    {
        try
        {
            _logger.LogInfo($"Начинаем синхронизацию с XML файлами...");

            Directory.CreateDirectory(_config.XmlFolderPath);
            Directory.CreateDirectory(_config.ArchiveFolderPath);

            foreach (var filePath in Directory.GetFiles(_config.XmlFolderPath, "*.xml"))
                await ProcessXmlFile(filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Произошла общая ошибка при обработке XML: {ex}");
        }
    }

    private async Task ProcessXmlFile(string filePath)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var customerPersistanceService = scope.ServiceProvider.GetRequiredService<ICustomerPersistenceService>();

            var rawList = await GetRawData(filePath);
            if (rawList?.Customers == null || !rawList.Customers.Any())
            {
                _logger.LogError($"Не удалось получить данные из файла {filePath}.");
                return;
            }

            var dtos = CreateDtoFromRaw(rawList.Customers);
            await customerPersistanceService.ProcessCustomerBatchAsync(dtos);

            var archivePath = ArchiveFile(filePath);
            _logger.LogInfo($"Файл успешно обработан и перенесен в директорию архивов {archivePath}.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Произошла ошибка при обработке XML файла {filePath}: {ex.Message}");
        }
    }

    private List<CustomerDto> CreateDtoFromRaw(List<CustomerRawDto> raws)
    {
        var dtoList = new List<CustomerDto>(raws.Count);

        foreach (var raw in raws)
        {
            var dto = new CustomerDto
            {
                CardCode = raw.CardCode,

                LastName = RowDataParsingHelper.NormalizeName(raw.LastName),
                FirstName = RowDataParsingHelper.NormalizeName(raw.FirstName),
                SurName = RowDataParsingHelper.NormalizeName(raw.SurName),

                Gender = RowDataParsingHelper.ParseGender(raw.Gender),
                Birthday = RowDataParsingHelper.ParseDate(raw.Birthday),
                City = RowDataParsingHelper.NormalizeCity(raw.City),

                Contacts = new ContactsDto
                {
                    Email = raw.Email,
                    PhoneMobile = RowDataParsingHelper.ParsePhoneMobile(raw.PhoneMobile)
                },

                FinancialProfile = new FinancialProfileDto
                {
                    Pincode = raw.Pincode,
                    Bonus = RowDataParsingHelper.ParseDecimal(raw.Bonus),
                    Turnover = RowDataParsingHelper.ParseDecimal(raw.Turnover)
                }
            };

            dtoList.Add(dto);
        }

        return dtoList;
    }

    private string ArchiveFile(string filePath)
    {
        var archivePath = Path.Combine(_config.ArchiveFolderPath, Path.GetFileName(filePath));
        File.Move(filePath, archivePath, overwrite: true);

        return archivePath;
    }

    private async Task<CustomerRawDtoList> GetRawData(string filePath)
    {
        using var reader = new StreamReader(filePath);
        var xmlContent = await reader.ReadToEndAsync();
        var rawList = await _serealizer.DeserializeObjectAsync<CustomerRawDtoList>(xmlContent);

        return rawList;
    }
}
