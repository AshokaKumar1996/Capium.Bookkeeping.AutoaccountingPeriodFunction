using Capium.Bookkeeping.AutoaccountingPeriodFunction.Configuration;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Services
{
    public class TemplateGenerator : ITemplateGenerator
    {
        private readonly MySettings _mySettings;

        public TemplateGenerator(MyConfiguration configuration)
        {
            _mySettings = configuration.Settings;
        }

        public string GetEmailTemplate(string bannerImage, string message)
        {
            var baseUrl = _mySettings.AzureKeyVaultValues.BookkeepingBaseUrl;
            return $"<html><body><img src='{baseUrl}/images/{bannerImage}' alt='banner' />{message}</body></html>";
        }
    }
}