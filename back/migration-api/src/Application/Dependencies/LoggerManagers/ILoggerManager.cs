using System.Runtime.CompilerServices;

namespace Application.Dependencies.Logging;

public interface ILoggerManager
{
    void LogInfo(string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0);
    void LogError(string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0);
    void LogFile(string message, string fileName, string format = "txt");
}
