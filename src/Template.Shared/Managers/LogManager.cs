using Microsoft.Extensions.Logging;
using Template.Shared.Managers.Interfaces;

namespace Template.Shared.Managers
{
    public class LogManager : ILogManager
    {
        private readonly ILogger<LogManager> _logger;

        public LogManager(ILogger<LogManager> logger)
        {
            _logger = logger;
        }

        public void Log(LogLevel level, string message, Exception? exception = null, object? payload = null)
        {
            var logMessage = FormatMessage(message, payload);

            switch (level)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _logger.LogDebug(exception, logMessage);
                    break;
                case LogLevel.Information:
                    _logger.LogInformation(exception, logMessage);
                    break;
                case LogLevel.Warning:
                    _logger.LogWarning(exception, logMessage);
                    break;
                case LogLevel.Error:
                    _logger.LogError(exception, logMessage);
                    break;
                case LogLevel.Critical:
                    _logger.LogCritical(exception, logMessage);
                    break;
                case LogLevel.None:
                default:
                    _logger.LogInformation(exception, logMessage);
                    break;
            }
        }

        private static string FormatMessage(string message, object? payload)
        {
            return $"{message} {(payload != null ? $"| Payload: {System.Text.Json.JsonSerializer.Serialize(payload)}" : "")}";
        }

        public void Info(string message, object? payload = null)
        {
            Log(LogLevel.Information, message, null, payload);
        }

        public void Warn(string message, object? payload = null)
        {
            Log(LogLevel.Warning, message, null, payload);
        }

        public void Error(string message, Exception? exception = null, object? payload = null)
        {
            Log(LogLevel.Error, message, exception, payload);
        }

        public void Fatal(string message, Exception? exception = null, object? payload = null)
        {
            Log(LogLevel.Critical, message, exception, payload);
        }
    }
}
