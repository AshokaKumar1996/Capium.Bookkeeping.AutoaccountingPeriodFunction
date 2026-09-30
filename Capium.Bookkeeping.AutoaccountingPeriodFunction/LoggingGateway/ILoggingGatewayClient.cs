using System.Threading;
using System.Threading.Tasks;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.LoggingGateway
{
    public interface ILoggingGatewayClient
    {
        Task<bool> PostLogAsync(LogEntryRequest request, CancellationToken ct = default);
    }
}
