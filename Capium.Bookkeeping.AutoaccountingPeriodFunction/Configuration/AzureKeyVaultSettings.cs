namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Configuration
{
    public sealed class AzureKeyVaultSettings
    {
        public string? AzureKeyVaultEndpoint { get; set; }
        public string? AzureKeyVaultSharedEndpoint { get; set; }
        public string? AzureDedicatedKeyVaultEndpoint { get; set; }
        public string? AzureKeyVaultDedicatedEndpoint { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public bool UseClientCredentials { get; set; }
        public string? IdentityServerCertificateName { get; set; }
        public string? DataProtectionKeyIdentifier { get; set; }
        public bool ReadConfigurationFromKeyVault { get; set; }
        public string? TenantId { get; set; }
    }
}
