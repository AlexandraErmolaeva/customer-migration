namespace Infrastructure.HostedServices.Config;

public abstract class BaseConfig
{
    public TimeSpan RepeatInterval { get; set; }
    public bool IsEnabled { get; set; }
}
