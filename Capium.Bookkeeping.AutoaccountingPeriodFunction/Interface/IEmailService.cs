using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.Email;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync<TData>(TData data, Guid companyId) where TData : EmailModel;

      

        Task<bool> SendReminderEmailAsync(EmailModel emailModel, long businessId);

        Task<bool> SendAsync(EmailModel emailModel, long businessId);
    }
}