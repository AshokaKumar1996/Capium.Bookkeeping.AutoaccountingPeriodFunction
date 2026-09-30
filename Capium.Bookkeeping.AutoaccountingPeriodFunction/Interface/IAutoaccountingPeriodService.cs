using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.Email;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface
{
    public interface IAutoaccountingPeriodService
    {
        Task ProcessAutoAccountingPeriodAsync(ExecutionReportModel executionReportModel);

        Task SendReminderfunemail(string status, string content, ExecutionReportModel executionReportModel);

        Task<bool> SendReminderEmailAsync(string status, string message, ExecutionReportModel executionReportModel);
    }
}
