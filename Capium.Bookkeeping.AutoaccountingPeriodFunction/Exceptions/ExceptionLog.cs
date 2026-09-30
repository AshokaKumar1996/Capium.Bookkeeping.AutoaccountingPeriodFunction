using Capium.Bookkeeping.AutoaccountingPeriodFunction.LoggingGateway;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Exceptions
{
    public class ExceptionLog : IExceptionLog
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor? _httpContextAccessor;
        private readonly ILoggingGatewayClient _loggingGatewayClient;
        private readonly LoggingGatewayOptions _loggingGatewayOptions;
        private readonly ILogger<ExceptionLog> _logger;

        public ExceptionLog(
            IConfiguration config,
            ILoggingGatewayClient loggingGatewayClient,
            IOptions<LoggingGatewayOptions> loggingGatewayOptions,
            ILogger<ExceptionLog> logger,
            IHttpContextAccessor? httpContextAccessor = null)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _loggingGatewayClient = loggingGatewayClient;
            _loggingGatewayOptions = loggingGatewayOptions.Value;
            _logger = logger;
        }

        public async Task LogExceptions(Exception ex)
        {
            await LogExceptions(ex, string.Empty);
        }

        public async Task LogExceptions(Exception ex, string message)
        {
            var combined = string.IsNullOrWhiteSpace(message)
                ? ex.Message
                : $"{message}{Environment.NewLine}Exception: {ex.Message}{Environment.NewLine}";

            await PostAsync(LoggingGateway.LogLevel.Error, combined, ex);
        }

        public async Task LogInformations(string str)
        {
            await PostAsync(LoggingGateway.LogLevel.Information, str, null);
        }

        public async Task LogRequestResponse(int statusCode, string url, string request, string response)
        {
            string logMessage = string.Empty;
            logMessage += $"StatusCode:{statusCode} \n";
            logMessage += $"URL:{url} \n";
            logMessage += $"Request:{request} \n";
            logMessage += $"Response:{response} \n";
            await PostAsync(LoggingGateway.LogLevel.Information, logMessage, null);
        }

        private async Task PostAsync(LoggingGateway.LogLevel level, string message, Exception? exception)
        {
            var datetime = DateTime.UtcNow;
            var unixTime = ((DateTimeOffset)datetime).ToUnixTimeSeconds();

            var correlationId =
                _httpContextAccessor?.HttpContext?.Items?["CorrelationId"]?.ToString()
                ?? _httpContextAccessor?.HttpContext?.TraceIdentifier;

            var hostName = _config.GetSection("BKFuncApiURL").Value
                ?? Environment.MachineName;

            var fullMessage = exception == null
                ? message
                : $"{message}{Environment.NewLine}Exception: {exception}{Environment.NewLine}";

            var properties = exception == null
                ? null
                : new Dictionary<string, object>
                {
                    ["exceptionType"] = exception.GetType().FullName ?? exception.GetType().Name
                };

            var request = new LogEntryRequest(
                Level: (int)level,
                ModuleId: _loggingGatewayOptions.ModuleId,
                Message: fullMessage,
                Service: _loggingGatewayOptions.Service,
                ServiceId: 122,
                HostName: hostName,
                CorrelationId: correlationId,
                EventTime: unixTime,
                Properties: properties
            );

            try
            {
                var posted = await _loggingGatewayClient.PostLogAsync(request).ConfigureAwait(false);
                if (!posted)
                {
                    _logger.LogWarning("Logging gateway returned non-success result for level {Level}.", level);
                }
            }
            catch (Exception gatewayEx)
            {
                _logger.LogWarning(gatewayEx, "Logging gateway call failed. Original log message was: {Message}", message);
            }
        }
    }
}
