using Capium.Bookkeeping.AutoaccountingPeriodFunction.Exceptions;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.Email;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Threading.Tasks;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction
{
    public class AutoaccountingPeriodFunction
    {
        private readonly ILoggingFrameworkService _loggingFrameworkService;
        private readonly IAutoaccountingPeriodService _autoaccountingPeriodService;
        private readonly IExceptionLog _exceptionLog;

        public AutoaccountingPeriodFunction(
            ILoggingFrameworkService loggerFactory,
            IAutoaccountingPeriodService autoaccountingPeriodService,
            IExceptionLog exceptionLog)
        {
            _loggingFrameworkService = loggerFactory;
            _autoaccountingPeriodService = autoaccountingPeriodService;
            _exceptionLog = exceptionLog;
        }

        [Function("AutoaccountingPeriodFunction")]
        public async Task Run([TimerTrigger("%FunctionTimer%")] MyInfo myTimer, FunctionContext executionContext)
        {
            await _exceptionLog.LogInformations($"Auto accounting period function started at: {DateTime.Now}");
            await _loggingFrameworkService.WriteInformationLogAsync(
                "Auto Accounting Period",
                $"Auto accounting period function started at: {DateTime.Now}");

            ExecutionReportModel executionReportModel = new()
            {
                StartTime = DateTime.Now
            };

            try
            {
                await _autoaccountingPeriodService.ProcessAutoAccountingPeriodAsync(executionReportModel);
                executionReportModel.EndTime = DateTime.Now;
                await _autoaccountingPeriodService.SendReminderfunemail("Success", string.Empty, executionReportModel);
            }
            catch (Exception ex)
            {
                await _loggingFrameworkService.WriteErrorLogAsync(
                    "Auto Accounting Period",
                    $"Message: {ex.Message} InnerException: {ex.InnerException?.Message} StackTrace: {ex.StackTrace}");

                executionReportModel.EndTime = DateTime.Now;
                await _autoaccountingPeriodService.SendReminderfunemail(
                    "Error",
                    ex.Message + " " + ex.InnerException,
                    executionReportModel);

                await _exceptionLog.LogExceptions(ex);
            }

            await _exceptionLog.LogInformations($"Auto accounting period function ended at: {DateTime.Now}");
            await _loggingFrameworkService.WriteInformationLogAsync(
                "Auto Accounting Period",
                $"Auto accounting period function ended at: {DateTime.Now}");
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
