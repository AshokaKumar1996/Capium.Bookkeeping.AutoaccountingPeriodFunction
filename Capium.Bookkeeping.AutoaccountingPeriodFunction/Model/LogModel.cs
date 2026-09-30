namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Model
{
    public class LogModel
    {
        public long EventTime { get; set; }

        public string Service { get; set; } = string.Empty;

        public string LogType { get; set; } = string.Empty;

        public string HostName { get; set; } = string.Empty;

        public int ServiceId { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
