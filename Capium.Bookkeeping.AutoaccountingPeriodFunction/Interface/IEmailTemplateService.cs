using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.Email;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface
{
    public interface IEmailTemplateService
    {
        Task<TemplateDetailModel> GetEmailTemplateAsync<TData>(
            string templateName,
            Guid companyId,
            TData data) where TData : EmailModel;

        string GetReminderEmailTemplateWithHeaderAndFooter(
            string status,
            string content,
            ExecutionReportModel executionReportModel);
    }
}