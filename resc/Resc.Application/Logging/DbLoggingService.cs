using Microsoft.AspNetCore.Http;
using Resc.Application.Common.Interfaces;
using Resc.Data.Logs;
using Resc.Data.Logs.Enums;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;

namespace Resc.Application.Logging
{
    public class DbLoggingService : ILoggingService
    {
        private readonly IAppLogContext context;

        public DbLoggingService(IAppLogContext context)
        {
            this.context = context;
        }

        public async Task LogExceptionAsync(Exception ex, HttpRequest request = null)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            await this.LogAsync(LogType.ExceptionLog, ex.Message, request);
        }

        public async Task LogRequestAsync(HttpRequest request)
        {
            await this.LogAsync(LogType.TraceLog, null, request);
        }

        private async Task LogAsync(LogType type, string message, HttpRequest request = null)
        {
            var log = new Log {
                Type = type,
                LogDate = DateTime.UtcNow,
                IP = request?.HttpContext.Connection.RemoteIpAddress.ToString(),
                Verb = request?.Method,
                Url = request?.GetDisplayUrl(),
                UserAgent = request?.Headers["User-Agent"].ToString(),
                Message = message
                //TODO USERID
            };

            this.context.Set<Log>().Add(log);
            await this.context.SaveChangesAsync();
        }
    }
}
