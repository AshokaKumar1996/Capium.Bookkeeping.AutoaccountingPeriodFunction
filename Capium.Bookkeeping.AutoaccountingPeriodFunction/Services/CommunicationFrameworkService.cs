using Capium.Bookkeeping.AutoaccountingPeriodFunction.Configuration;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.Email;
using RestSharp;
using System.Net;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Services
{
    public class CommunicationFrameworkService : ICommunicationFrameworkService
    {
        private readonly MySettings _mySettings;

        public CommunicationFrameworkService(MyConfiguration myConfiguration)
        {
            _mySettings = myConfiguration.Settings;
        }

        public async Task<RestResponse> SendEmailFromCommunicationAsync(EmailModel emailModel)
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            var client = new RestClient(_mySettings.AzureKeyVaultValues.CommunicationAPIUrl);
            var request = new RestRequest("api/Email/SendEmailFromAzureFunctions", Method.Post);

            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            request.AddParameter(
                "application/json",
                Newtonsoft.Json.JsonConvert.SerializeObject(emailModel),
                ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);
            return response;
        }
    }
}