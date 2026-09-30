namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.LoggingGateway
{
    public class LoggingGatewayOptions
    {
        public const string SectionName = "LoggingGateway";
        public const string LegacySectionName = "LoggingApi";

        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// Audience / scope for token acquisition. Accepts either:
        /// - "api://{clientId}" (will be normalized to "api://{clientId}/.default")
        /// - "api://{clientId}/.default"
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        public int ModuleId { get; set; } = 36;

        public string Service { get; set; } = "Bookkeeping_AutoaccountingPeriod_Func";

        /// <summary>
        /// For local/dev environments where gateway auth is disabled.
        /// </summary>
        public bool RequireAuthentication { get; set; } = true;
    }
}
