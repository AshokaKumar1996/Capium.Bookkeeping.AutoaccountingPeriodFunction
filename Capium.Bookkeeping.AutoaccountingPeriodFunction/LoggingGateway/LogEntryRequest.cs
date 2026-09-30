using System.Collections.Generic;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.LoggingGateway
{
    public record LogEntryRequest(
        int Level,
        int ModuleId,
        string Message,
        string Service = null,
        int? ServiceId = null,
        string HostName = null,
        string CorrelationId = null,
        long? EventTime = null,
        Dictionary<string, object> Properties = null
    );
}
