using Application.Dependencies.Logging;
using Infrastructure.ApplicationDependencies.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Settings.Configuration;
using Serilog.Sinks.SystemConsole.Themes;

namespace CustomerMigrationApi.Confugirations;

public static class LoggerConfiguration
{
    public static void AddLogger(this WebApplicationBuilder builder)
    {
        var appDir = AppContext.BaseDirectory;
        var defaultOutput = "{Timestamp:HH:mm:ss} [{Level}] [{CallerClass}: in {CallerMember}() {CallerLine}]: {Message}{NewLine}{Exception}";

        builder.Host
            .UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration,
            new ConfigurationReaderOptions()
            {
                SectionName = "Serilog"
            })
            .Enrich.FromLogContext()

            // Вывод в консоль. Исключаем вывод для LogFile Subfolder.
            .WriteTo.Logger(loggerConfig =>
                loggerConfig
                    .Filter.ByExcluding(e => e.Properties.ContainsKey("Subfolder"))
                    .WriteTo.Console(
                        theme: AnsiConsoleTheme.Sixteen,
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] <s:{CallerClass} in {CallerMember}()> {Message:lj}{NewLine}{Exception}"))

            // Пишем в отдельный файл, LogFile.
            .WriteTo.Logger(loggerConfig =>
            {
                loggerConfig
                .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("Subfolder"))
                .WriteTo.Map(
                    keyPropertyName: "Subfolder_FileName",
                    defaultKey: "General_default.txt",
                    configure: (key, wt) =>
                    {
                        wt.File(
                            path: Path.Combine(appDir, "logs", $"{DateTime.Now:yyyy-MM-dd}/{key}"),
                            outputTemplate: "{Message}{NewLine}{Exception}"
                        );
                    });
            })

            // Ошибки.
            .WriteTo.Logger(loggerConfig =>
            {
                loggerConfig
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                    .WriteTo.Map(logEvent => logEvent.Timestamp.Date,
                    (date, writeTo) => writeTo.File(
                            path: Path.Combine(appDir, "logs", $"{date:yyyy-MM-dd}/error_log.txt"),
                            outputTemplate: defaultOutput));

            })

            // Пишем инфо в файл.
            .WriteTo.Logger(loggerConfig =>
            {
                loggerConfig
                    .Filter.ByExcluding(e => e.Level == LogEventLevel.Error || e.Properties.ContainsKey("Subfolder") || (e.Properties.ContainsKey("SourceContext")
                    && e.Properties["SourceContext"].ToString().Contains("Microsoft.EntityFrameworkCore.Database.Command")))
                    .WriteTo.Map(logEvent => logEvent.Timestamp.Date,
                    (date, writeTo) => writeTo.File(
                    path: Path.Combine(appDir, "logs", $"{date:yyyy-MM-dd}/logs.txt"),
                    outputTemplate: defaultOutput));
            })
         );

        builder.Services.AddSingleton<ILoggerManager, SerilogManager>();
    }
}
