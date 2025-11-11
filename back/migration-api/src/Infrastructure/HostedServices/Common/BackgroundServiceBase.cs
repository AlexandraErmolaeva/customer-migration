using Application.Dependencies.Logging;
using Infrastructure.HostedServices.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infrastructure.HostedServices.Common;

public abstract class BackgroundServiceBase : BackgroundService
{
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ILoggerManager _logger;

    private readonly TimeSpan _repeatInterval;
    private readonly bool _isEnabled;

    protected BackgroundServiceBase(IHostApplicationLifetime lifetime, ILoggerManager logger, IOptions<BaseConfig> config)
    {
        _lifetime = lifetime;
        _isEnabled = config.Value.IsEnabled;
        _repeatInterval = config.Value.RepeatInterval;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_isEnabled)
            return;

        if (!await WaitForAppStartup(_lifetime, stoppingToken))
            return;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ToDo();
            }
            catch (Exception ex)
            {
                _logger.LogInfo($"{GetType().Name} - Ошибка итерации {ex}.");
            }
            finally
            {
                await Task.Delay(_repeatInterval);
            }
        }
    }

    static async Task<bool> WaitForAppStartup(IHostApplicationLifetime lifetime, CancellationToken stoppingToken)
    {
        var startedSource = new TaskCompletionSource();
        using var reg1 = lifetime.ApplicationStarted.Register(() => startedSource.SetResult());

        var cancelledSource = new TaskCompletionSource();
        using var reg2 = stoppingToken.Register(() => cancelledSource.SetResult());

        // Ожидаем любое из событий запуска или запроса на остановку.
        var completedTask = await Task.WhenAny(startedSource.Task, cancelledSource.Task).ConfigureAwait(false);

        return completedTask == startedSource.Task;
    }

    /// <summary>
    /// Функция, выполнение которой производится с указанным периодом.
    /// </summary>
    public abstract Task ToDo();
}
