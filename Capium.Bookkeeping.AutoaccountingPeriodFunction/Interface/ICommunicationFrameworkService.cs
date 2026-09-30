using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.Email;
using RestSharp;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface
{
    public interface ICommunicationFrameworkService
    {
        Task<RestResponse> SendEmailFromCommunicationAsync(EmailModel emailModel);
    }
}