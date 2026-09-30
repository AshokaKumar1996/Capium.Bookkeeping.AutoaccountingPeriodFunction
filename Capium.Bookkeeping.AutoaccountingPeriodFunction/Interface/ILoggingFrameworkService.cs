using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model;
using RestSharp;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface
{
    public interface ILoggingFrameworkService
    {
        Task<RestResponse?> WriteLogAsync(LogModelExp logModel);

        Task WriteInformationLogAsync(string methodName, string message);

        Task WriteErrorLogAsync(string methodName, string message);
    }
}
