using Microsoft.Extensions.Logging;

namespace Template.Infrastructure.Logging
{
    public interface ILogManager<T>
    {
        void Log(LogLevel level, string message, Exception? exception = null, object? payload = null);
        void Info(string message, object? payload = null);
        void Warn(string message, object? payload = null);
        void Error(string message, Exception? exception = null, object? payload = null);
        void Fatal(string message, Exception? exception = null, object? payload = null);
    }
}
