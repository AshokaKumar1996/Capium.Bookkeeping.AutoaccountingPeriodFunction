namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Model
{
    public class LogModelExp
    {
        public DateTime EventTime { get; set; }

        public int LogLevel { get; set; }

        public string CorrelationId { get; set; } = string.Empty;

        public int StatusCode { get; set; }

        public string RequestPath { get; set; } = string.Empty;

        public string Application { get; set; } = string.Empty;

        public string ServiceId { get; set; } = string.Empty;

        public string MethodName { get; set; } = string.Empty;

        public string MethodType { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}
