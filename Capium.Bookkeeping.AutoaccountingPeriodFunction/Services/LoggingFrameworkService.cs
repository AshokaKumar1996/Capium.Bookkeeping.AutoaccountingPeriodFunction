using Capium.Bookkeeping.AutoaccountingPeriodFunction.Configuration;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Enums;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model;
using RestSharp;
using System.Net;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Services
{
    public class LoggingFrameworkService : ILoggingFrameworkService
    {
        private readonly MySettings _mySettings;

        public LoggingFrameworkService(MyConfiguration myConfiguration)
        {
            _mySettings = myConfiguration.Settings;
        }

        public async Task<RestResponse?> WriteLogAsync(LogModelExp logModel)
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            if (string.IsNullOrWhiteSpace(_mySettings.AzureKeyVaultValues.LoggingFrameWorkAPIBaseUrl))
            {
                return null;
            }

            var client = new RestClient(_mySettings.AzureKeyVaultValues.LoggingFrameWorkAPIBaseUrl);
            var request = new RestRequest("api/Log/writecustomlogazurefunctions", Method.Post);

            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            request.AddParameter(
                "application/json",
                Newtonsoft.Json.JsonConvert.SerializeObject(logModel),
                ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);
            return response;
        }

        public async Task WriteInformationLogAsync(string methodName, string message)
        {
            var environment = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");

            await WriteLogAsync(new LogModelExp
            {
                Application = $"Bookkeeping {environment}",
                CorrelationId = Guid.NewGuid().ToString(),
                EventTime = DateTime.Now,
                LogLevel = (int)LogEventType.Information,
                Message = message,
                MethodName = methodName,
                RequestPath = AppDomain.CurrentDomain.BaseDirectory,
            });
        }

        public async Task WriteErrorLogAsync(string methodName, string message)
        {
            var environment = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");

            await WriteLogAsync(new LogModelExp
            {
                Application = $"Bookkeeping {environment}",
                CorrelationId = Guid.NewGuid().ToString(),
                EventTime = DateTime.Now,
                LogLevel = (int)LogEventType.Error,
                Message = message,
                MethodName = methodName,
                RequestPath = AppDomain.CurrentDomain.BaseDirectory,
            });
        }
    }
}
