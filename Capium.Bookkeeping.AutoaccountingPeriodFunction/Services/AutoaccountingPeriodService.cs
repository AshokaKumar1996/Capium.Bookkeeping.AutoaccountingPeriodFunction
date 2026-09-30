using Capium.Bookkeeping.AutoaccountingPeriodFunction.Configuration;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Exceptions;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.AccountingPeriod;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.Email;
using Microsoft.Extensions.Logging;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Services
{
    public class AutoaccountingPeriodService : IAutoaccountingPeriodService
    {
        private readonly ILogger<AutoaccountingPeriodService> _logger;
        private readonly IExceptionLog _exceptionLog;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly MySettings _mySettings;
        private readonly IClientDetailsRepository _clientDetailsRepository;
        private readonly IAccountingPeriodRepository _accountingPeriodRepository;

        public AutoaccountingPeriodService(
            ILogger<AutoaccountingPeriodService> logger,
            IClientDetailsRepository clientDetailsRepository,
            IAccountingPeriodRepository accountingPeriodRepository,
            IExceptionLog exceptionLog,
            IEmailService emailService,
            IEmailTemplateService emailTemplateService,
            MyConfiguration myConfiguration)
        {
            _logger = logger;
            _clientDetailsRepository = clientDetailsRepository;
            _accountingPeriodRepository = accountingPeriodRepository;
            _exceptionLog = exceptionLog;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
            _mySettings = myConfiguration.Settings;
        }

        public async Task ProcessAutoAccountingPeriodAsync(ExecutionReportModel executionReportModel)
        {
            var clients = await _clientDetailsRepository.GetAutoAccountingPeriodClientsAsync();

            var totalCreated = 0;

            foreach (var client in clients)
            {
                if (client.ClientId is null || client.ClientId == Guid.Empty)
                {
                    continue;
                }

                _logger.LogInformation(
                    "AutoAccounting Client => ClientId: {ClientId}, UniversalId: {UniversalId}, AccountingMethodId: {AccountingMethodId}, IsBranchDepartmentAccounting: {IsBranchDepartmentAccounting}, UpdatedOn: {UpdatedOn}",
                    client.ClientId,
                    client.UniversalId,
                    client.AccountingMethodId,
                    client.IsBranchDepartmentAccounting,
                    client.UpdatedOn);

                var lastAccountingPeriod = await _accountingPeriodRepository.GetLatestByCompanyIdAsync(client.ClientId.Value);
                if (lastAccountingPeriod is null)
                {
                    _logger.LogInformation("No accounting periods found for client {ClientId}", client.ClientId);
                    continue;
                }

                totalCreated += await CreateMissingPeriodsUntilCurrentAsync(lastAccountingPeriod, client.ClientId.Value, client.UpdatedBy);
            }

            executionReportModel.NumberOfRecordsFound = clients.Count;
            executionReportModel.NumberOfRecordsCreated = totalCreated;
            executionReportModel.ExecutionDate = DateTime.UtcNow;
            executionReportModel.AzureFunctionName = "AutoaccountingPeriodFunction";
            executionReportModel.EnvironmentName = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") ?? "Unknown";

            if (clients.Count == 0)
            {
                _logger.LogInformation("No clients found with IsAutoAccountingPeriod = 1.");
            }
            else
            {
                _logger.LogInformation("Found {Count} clients with IsAutoAccountingPeriod = 1.", clients.Count);
                _logger.LogInformation("Created {Count} new accounting periods.", totalCreated);
            }
        }

        private async Task<int> CreateMissingPeriodsUntilCurrentAsync(AccountingPeriodModel lastPeriod, Guid companyId, Guid? updatedBy)
        {
            var today = GetCurrentUkDate();

            if (lastPeriod.IsCeased)
            {
                _logger.LogInformation("Last accounting period is ceased for client {ClientId}. Auto creation skipped.", companyId);
                return 0;
            }

            if (today >= lastPeriod.FromDate.Date && today <= lastPeriod.ToDate.Date)
            {
                return 0;
            }

            if (lastPeriod.ToDate.Date < lastPeriod.FromDate.Date)
            {
                return 0;
            }

            var createdCount = 0;
            var currentFromDate = lastPeriod.FromDate.Date;
            var currentToDate = lastPeriod.ToDate.Date;

            while (today > currentToDate)
            {
                var nextFromDate = currentFromDate.AddYears(1);
                var nextToDate = nextFromDate.AddYears(1).AddDays(-1);

                var newPeriod = new AccountingPeriodModel
                {
                    FromDate = nextFromDate,
                    ToDate = nextToDate,
                    CompanyId = companyId,
                    IsLocked = false,
                    IsCeased = false,
                    IsComparative = false,
                    IsDeleted = false,
                    CreatedBy = updatedBy
                };

                var insertedId = await _accountingPeriodRepository.CreateAsync(newPeriod);
                if (insertedId != Guid.Empty)
                {
                    createdCount++;
                    _logger.LogInformation(
                        "Created accounting period for client {ClientId}: {FromDate} - {ToDate}",
                        companyId,
                        nextFromDate,
                        nextToDate);
                }

                currentToDate = nextToDate;
                currentFromDate = nextFromDate;
            }

            return createdCount;
        }

        private static DateTime GetCurrentUkDate()
        {
            var ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ukTimeZone).Date;
        }

        public async Task SendReminderfunemail(string status, string content, ExecutionReportModel executionReportModel)
        {
            await _exceptionLog.LogInformations($"1. SendReminderfunemail function started at: {DateTime.Now}");
            await SendReminderEmailAsync(status, content, executionReportModel);
            await _exceptionLog.LogInformations($"8. SendReminderfunemail ExecuteAsync function end at: {DateTime.Now}");
        }

        public async Task<bool> SendReminderEmailAsync(string status, string message, ExecutionReportModel executionReportModel)
        {
            bool emailStatus;

            try
            {
                await _exceptionLog.LogInformations("2. Inside SendReminderEmailAsync Started");
                await _exceptionLog.LogInformations($"3. SendReminderEmailAsync function started at: {DateTime.Now}");

                EmailModel emailModel = new()
                {
                    //ToEmail = new() { Convert.ToString(_mySettings.AzureKeyVaultValues.ITSupportEmail) },
                    ToEmail = new() { Convert.ToString("ashokabehera829@gmail.com") },
                    ToName = "Capium",
                    FromName = "Capium",
                    IsSendFromCapium = true,
                    Message = _emailTemplateService.GetReminderEmailTemplateWithHeaderAndFooter(status, message, executionReportModel),
                    Subject = "Azure Function Execution Report: func-AutoaccountingPeriod"
                };

                await _exceptionLog.LogInformations($"4. SendReminderEmailAsync sending email to : {string.Join(",", emailModel.ToEmail)}");

                emailStatus = await _emailService.SendReminderEmailAsync(emailModel, 0);

                await _exceptionLog.LogInformations($"5. SendReminderEmailAsync sending email status : {emailStatus}");
            }
            catch (Exception ex)
            {
                await _exceptionLog.LogExceptions(ex);
                await _exceptionLog.LogInformations($"6 Timer trigger Error found ex.Message:{ex.Message}, ex.InnerException{ex.InnerException}");
                return false;
            }

            await _exceptionLog.LogInformations("7. Inside SendReminderEmailAsync Started");
            return emailStatus;
        }
    }
}
