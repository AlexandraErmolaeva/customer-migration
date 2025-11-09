using Application.Dependencies.Logging;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using System.Runtime.CompilerServices;

namespace Infrastructure.ApplicationDependencies.Logging;

public class SerilogManager : ILoggerManager
{
    private readonly ILogger<SerilogManager> _logger;

    public SerilogManager(ILogger<SerilogManager> logger)
    {
        _logger = logger;
    }

    public delegate void LogDelegate(string? message, params object?[] args);

    public void LogError(string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
    {
        FormLog(_logger.LogError, message, callerFile, callerMember, callerLine);
    }

    public void LogInfo(string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
    {
        FormLog(_logger.LogInformation, message, callerFile, callerMember, callerLine);
    }

    public void LogFile(string message, string fileName, string format = "txt")
    {
        var time = DateTime.Now.ToString("HH_mm_ss_ff");
        var fullFileName = fileName + "_" + time + "." + format.ToLower().Trim('.');

        var subfolder = GetSubfolder(fileName);

        var combinedKey = $"{subfolder}/{fullFileName}";
        using (LogContext.PushProperty("Subfolder_FileName", combinedKey))
        {
            Log.Logger.ForContext("FileName", fullFileName)
                      .ForContext("Subfolder", subfolder)
                      .Information(message);
        }
    }

    private string GetSubfolder(string fileName)
    {
        var subfolder = string.Empty;
        switch (fileName)
        {
            case "XML":
                subfolder = "XML";
                break;
            default:
                subfolder = "General";
                break;
        }
        return subfolder;
    }

    private void FormLog(LogDelegate action, string message, string callerFile, string callerMember, int callerLine)
    {
        var callerClass = Path.GetFileNameWithoutExtension(callerFile);

        using (LogContext.PushProperty("CallerClass", callerClass))
        using (LogContext.PushProperty("CallerMember", callerMember))
        using (LogContext.PushProperty("CallerLine", callerLine))
        {
            action(message);
        }
    }
}