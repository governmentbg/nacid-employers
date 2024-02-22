using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Resc.Application.Logging
{
    public interface ILoggingService
    {
        Task LogRequestAsync(HttpRequest request);

        Task LogExceptionAsync(Exception ex, HttpRequest request = null);
    }
}
