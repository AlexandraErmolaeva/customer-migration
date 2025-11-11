using Application.Dependencies.Logging;
using Infrastructure.HostedServices.Common;
using Infrastructure.HostedServices.Config;
using Infrastructure.HostedServices.Dtos;
using Infrastructure.InternalDependencies.Serializers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infrastructure.HostedServices;

public class XmlGenerator : BackgroundServiceBase
{
    private readonly ILoggerManager _logger;
    private readonly IXmlSerealizer _serializer;
    private readonly XmlGeneratorConfig _config;

    private const int FILES_COUNT = 3;
    public XmlGenerator(IXmlSerealizer serializer, IHostApplicationLifetime lifetime, ILoggerManager logger, IOptions<XmlGeneratorConfig> config) : base(lifetime, logger, config)
    {
        _logger = logger;
        _serializer = serializer;
        _config = config.Value;
    }

    public override async Task ToDo()
    {
        Directory.CreateDirectory(_config.XmlFolderPath);
        var random = new Random();
        var customers = new List<CustomerRawDto>();

        for (var i = 0; i < FILES_COUNT; i++)
        {
            _logger.LogInfo($"Начинаем генерацию {i} из {FILES_COUNT} файлов...");

            var gender = random.Next(0, 2) == 0 ? "Муж" : "Женский";
            var birthDate = new DateTime(
                random.Next(1950, 2000),
                random.Next(1, 12),
                random.Next(1, 28)
            );
            var cardCode = random.NextInt64(50000000, 90000000);

            customers.Add(new CustomerRawDto
            {
                CardCode = cardCode.ToString(),
                LastName = $"Фамилия{i}",
                FirstName = $"Имя{i}",
                SurName = $"Отчество{i}",
                Gender = gender,
                Birthday = birthDate.ToString("dd.MM.yyyy"),
                City = $"Город{random.Next(1, 10)}",
                PhoneMobile = $"+79{random.Next(100000000, 999999999)}",
                Email = $"client{i}@example.com",
                Pincode = random.Next(1000, 9999).ToString(),
                Bonus = (random.NextDouble() * 1000).ToString("F2"),
                Turnover = (random.NextDouble() * 5000).ToString("F2")
            });
        }

        var wrapper = new CustomerRawDtoList { Customers = customers };

        var fileName = $"customers_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xml";
        var filePath = Path.Combine(_config.XmlFolderPath, fileName);

        var xml = await _serializer.SerializeObjectAsync(wrapper);
        await File.WriteAllTextAsync(filePath, xml);
        _logger.LogInfo($"Сгенерирован XML файл: {filePath}");
    }
}
