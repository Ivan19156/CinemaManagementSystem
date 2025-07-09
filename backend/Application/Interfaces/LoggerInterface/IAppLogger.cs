using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.Infrastructure.Logging
{
    public interface IAppLogger<T>
    {
        void LogInfo(string message, params object[] args);
        void LogError(Exception ex, string message, params object[] args);
        void LogWarning(string v);
    }

}
