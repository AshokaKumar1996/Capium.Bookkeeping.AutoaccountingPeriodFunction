using Microsoft.Extensions.Options;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Configuration
{
    public class MyConfiguration
    {
        public MySettings Settings { get; set; }
        public MyConfiguration(
            IOptions<MySettings> options,
            AzureKeyVaultValues AzureKeyVaultValues)
        {
            Settings = options.Value;
            Settings.AzureKeyVaultValues = AzureKeyVaultValues;
        }
    }
    public class MySettings
    {
        public AzureKeyVaultValues AzureKeyVaultValues { get; set; }
    }
}
