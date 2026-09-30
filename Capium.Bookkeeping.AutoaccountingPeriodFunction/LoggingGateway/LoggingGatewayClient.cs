using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.LoggingGateway
{
    public class LoggingGatewayClient : ILoggingGatewayClient
    {
        private readonly HttpClient _httpClient;
        private readonly LoggingGatewayOptions _options;
        private readonly TokenCredential _credential;
        private readonly ILogger<LoggingGatewayClient> _logger;

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public LoggingGatewayClient(
            HttpClient httpClient,
            IOptions<LoggingGatewayOptions> options,
            ILogger<LoggingGatewayClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _credential = new DefaultAzureCredential();
        }

        public async Task<bool> PostLogAsync(LogEntryRequest request, CancellationToken ct = default)
        {
            var payloadJson = JsonSerializer.Serialize(request, SerializerOptions);

            using var req = new HttpRequestMessage(HttpMethod.Post, new Uri("/api/v2/logs", UriKind.Relative))
            {
                Content = new StringContent(payloadJson, Encoding.UTF8, "application/json")
            };

            string? scopeUsed = null;
            if (_options.RequireAuthentication)
            {
                if (string.IsNullOrWhiteSpace(_options.Audience))
                {
                    _logger.LogWarning(
                        "LoggingGateway audience is empty while authentication is required. Sending log request without bearer token.");
                }
                else
                {
                var scope = NormalizeScope(_options.Audience);
                scopeUsed = scope;

                var token = await _credential
                    .GetTokenAsync(new TokenRequestContext(new[] { scope }), ct)
                    .ConfigureAwait(false);

                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
                }
            }

            using var response = await _httpClient.SendAsync(req, ct).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            _logger.LogWarning(
                "Logging gateway returned non-success status {StatusCode}. Url: {Url}. Scope: {Scope}. Payload: {Payload}. Body: {Body}",
                (int)response.StatusCode,
                response.RequestMessage?.RequestUri?.ToString() ?? req.RequestUri?.ToString(),
                scopeUsed,
                payloadJson,
                body);

            return false;
        }

        private static string NormalizeScope(string audienceOrScope)
        {
            if (string.IsNullOrWhiteSpace(audienceOrScope))
            {
                throw new InvalidOperationException("LoggingGateway:Audience must be configured (e.g. api://{clientId}/.default).");
            }

            var value = audienceOrScope.Trim();
            if (value.EndsWith("/.default", StringComparison.OrdinalIgnoreCase))
            {
                return value;
            }

            return value.TrimEnd('/') + "/.default";
        }
    }
}
