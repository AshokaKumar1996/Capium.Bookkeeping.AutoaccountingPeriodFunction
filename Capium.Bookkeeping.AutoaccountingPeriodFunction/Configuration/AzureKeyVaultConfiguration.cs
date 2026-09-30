using Azure.Core;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Configuration
{
    public static class AzureKeyVaultConfiguration
    {
        public static void AddAzureKeyVaultConfig(
            this IConfiguration configuration,
            IConfigurationBuilder configurationBuilder)
        {
            var settingsSection = configuration.GetSection(nameof(AzureKeyVaultSettings));
            if (!settingsSection.Exists())
            {
                settingsSection = configuration.GetSection("AzureKeyVaultConfiguration");
            }

            if (settingsSection.Exists())
            {
                var azureKeyVaultConfiguration = settingsSection.Get<AzureKeyVaultSettings>();

                if (azureKeyVaultConfiguration != null && azureKeyVaultConfiguration.ReadConfigurationFromKeyVault)
                {
                    var sharedEndpoint = azureKeyVaultConfiguration.AzureKeyVaultEndpoint
                        ?? azureKeyVaultConfiguration.AzureKeyVaultSharedEndpoint;
                    var dedicatedEndpoint = azureKeyVaultConfiguration.AzureDedicatedKeyVaultEndpoint
                        ?? azureKeyVaultConfiguration.AzureKeyVaultDedicatedEndpoint;

                    if (azureKeyVaultConfiguration.UseClientCredentials)
                    {
                        var tenantId = azureKeyVaultConfiguration.TenantId
                            ?? configuration["AzureKeyVaultConfiguration:TenantId"];

                        if (!string.IsNullOrWhiteSpace(tenantId)
                            && !string.IsNullOrWhiteSpace(azureKeyVaultConfiguration.ClientId)
                            && !string.IsNullOrWhiteSpace(azureKeyVaultConfiguration.ClientSecret))
                        {
                            var credential = new ClientSecretCredential(
                                tenantId,
                                azureKeyVaultConfiguration.ClientId,
                                azureKeyVaultConfiguration.ClientSecret);

                            AddKeyVault(configurationBuilder, sharedEndpoint, credential);
                            AddKeyVault(configurationBuilder, dedicatedEndpoint, credential);
                        }
                        else
                        {
                            AddAzureKeyVaultWithManagedIdentity(configuration, configurationBuilder);
                        }
                    }
                    else
                    {
                        AddAzureKeyVaultWithManagedIdentity(configuration, configurationBuilder);
                    }
                }
            }
        }

        public static void AddAzureKeyVaultWithManagedIdentity(this IConfiguration configuration, IConfigurationBuilder configBuilder)
        {
            var sharedVault = configuration["AzureKeyVaultConfiguration:AzureKeyVaultEndpoint"]
                ?? configuration["AzureKeyVaultConfiguration:AzureKeyVaultSharedEndpoint"];
            var dedicatedVault = configuration["AzureKeyVaultConfiguration:AzureDedicatedKeyVaultEndpoint"]
                ?? configuration["AzureKeyVaultConfiguration:AzureKeyVaultDedicatedEndpoint"];
            var environment = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");
            string? tenantId = configuration["AzureKeyVaultConfiguration:TenantId"];

            TokenCredential credential;

            if (string.Equals(environment, "Local", StringComparison.OrdinalIgnoreCase)
                || string.Equals(environment, "Development", StringComparison.OrdinalIgnoreCase))
            {
                var cli = new AzureCliCredential();
                var interactive = new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions
                {
                    TenantId = tenantId
                });

                credential = new ChainedTokenCredential(cli, interactive);
            }
            else
            {
                credential = new ManagedIdentityCredential();
            }

            if (!string.IsNullOrWhiteSpace(sharedVault))
            {
                AddKeyVault(configBuilder, sharedVault, credential);
            }

            if (!string.IsNullOrWhiteSpace(dedicatedVault))
            {
                AddKeyVault(configBuilder, dedicatedVault, credential);
            }
        }

        private static void AddKeyVault(IConfigurationBuilder configBuilder, string vaultEndpoint, TokenCredential credential)
        {
            if (string.IsNullOrWhiteSpace(vaultEndpoint))
            {
                return;
            }

            configBuilder.AddAzureKeyVault(new Uri(vaultEndpoint), credential, new AzureKeyVaultConfigurationOptions());
        }

    }
}
