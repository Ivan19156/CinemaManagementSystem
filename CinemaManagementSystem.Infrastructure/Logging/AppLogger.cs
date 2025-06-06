using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.Infrastructure.Logging
{
    public class AppLogger<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;

        public AppLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message, params object[] args)
            => _logger.LogInformation(message, args);

        public void LogError(Exception ex, string message, params object[] args)
            => _logger.LogError(ex, message, args);

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

    }

}
