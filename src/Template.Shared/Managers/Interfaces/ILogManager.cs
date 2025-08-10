using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Shared.Managers.Interfaces
{
    public interface ILogManager
    {
        void Log(LogLevel level, string message, Exception? exception = null, object? payload = null);
        void Info(string message, object? payload = null);
        void Warn(string message, object? payload = null);
        void Error(string message, Exception? exception = null, object? payload = null);
        void Fatal(string message, Exception? exception = null, object? payload = null);
    }
}
