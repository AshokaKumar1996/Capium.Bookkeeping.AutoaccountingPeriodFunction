using Microsoft.Extensions.Configuration;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Configuration
{
    public class AzureKeyVaultValues
    {
        private readonly IConfiguration _configuration;

        public AzureKeyVaultValues(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string BKConnectionString { get => _configuration[nameof(BKConnectionString)]; }
        public string AzureBlobConnectionString { get => _configuration[nameof(AzureBlobConnectionString)]; }
        public string BKBlobContainerName { get => _configuration[nameof(BKBlobContainerName)]; }
        public string BKBlobPath { get => _configuration[nameof(BKBlobPath)]; }
        public string IdentityRedirectUrl { get => _configuration[nameof(IdentityRedirectUrl)]; }
        public string RedisCacheConnectionString { get => _configuration[nameof(RedisCacheConnectionString)]; }
        public string GoogleAPIFontSans { get => _configuration[nameof(GoogleAPIFontSans)]; }
        public string GoogleAPIFontBarlow { get => _configuration[nameof(GoogleAPIFontBarlow)]; }
        public string GoogleAPIFontSansPro { get => _configuration[nameof(GoogleAPIFontSansPro)]; }
        //public string TrueLayerBaseUrl { get => _configuration[nameof(TrueLayerBaseUrl)]; }
        //public string TrueLayerClientId { get => _configuration[nameof(TrueLayerClientId)]; }
        //public string TrueLayerClientSecret { get => _configuration[nameof(TrueLayerClientSecret)]; }
        public string AccessTokenUrl { get => _configuration[nameof(AccessTokenUrl)]; }
        //public string TrueLayerAccountUrl { get => _configuration[nameof(TrueLayerAccountUrl)]; }
        //public string BKTrueLayerRedirectUrl { get => _configuration[nameof(BKTrueLayerRedirectUrl)]; }
        public string GoogleClientId { get => _configuration[nameof(GoogleClientId)]; }
        public string GoogleClientSecret { get => _configuration[nameof(GoogleClientSecret)]; }
        public string GoogleAccountAuthUrl { get => _configuration[nameof(GoogleAccountAuthUrl)]; }
        public string GoogleRevokeTokenUrl { get => _configuration[nameof(GoogleRevokeTokenUrl)]; }
        public string GoogleApplicationName { get => _configuration[nameof(GoogleApplicationName)]; }
        public string GoogleExchangeCodeForTokenUrl { get => _configuration[nameof(GoogleExchangeCodeForTokenUrl)]; }
        public string GoogleGetEmailUrl { get => _configuration[nameof(GoogleGetEmailUrl)]; }
        public string BKGoogleRedirectUrl { get => _configuration[nameof(BKGoogleRedirectUrl)]; }
        public string MicrosoftAuthUrl { get => _configuration[nameof(MicrosoftAuthUrl)]; }
        public string MicrosofTokenUrl { get => _configuration[nameof(MicrosofTokenUrl)]; }
        public string MicrosoftScopeValue { get => _configuration[nameof(MicrosoftScopeValue)]; }
        public string Office365ClientId { get => _configuration[nameof(Office365ClientId)]; }
        public string BKOffice365RedirectUrl { get => _configuration[nameof(BKOffice365RedirectUrl)]; }
        public string Office365ClientSecret { get => _configuration[nameof(Office365ClientSecret)]; }
        public string SendGridAPIKey { get => _configuration[nameof(SendGridAPIKey)]; }
        public string SenderEmail { get => _configuration[nameof(SenderEmail)]; }
        public string SenderName { get => _configuration[nameof(SenderName)]; }
        public string BookkeepingBaseUrl { get => _configuration[nameof(BookkeepingBaseUrl)]; }

        public string LoggingFrameWorkAPIBaseUrl { get => _configuration[nameof(LoggingFrameWorkAPIBaseUrl)]; }
        public string ITSupportEmail { get => _configuration[nameof(ITSupportEmail)]; }
        public string AzureCDNUrl { get => _configuration[nameof(AzureCDNUrl)]; }
        public string CommunicationAPIUrl { get => _configuration[nameof(CommunicationAPIUrl)]; }
    }
}
