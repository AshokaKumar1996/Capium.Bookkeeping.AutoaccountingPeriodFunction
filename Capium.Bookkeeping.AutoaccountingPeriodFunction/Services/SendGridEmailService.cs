using Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.Email;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Services
{
    public class SendGridEmailService : ISendGridEmailService
    {
        public Task<bool> SendEmailAsync(EmailModel emailModel, Guid companyId)
        {
            return Task.FromResult(emailModel.IsSendFromCapium && emailModel.ToEmail.Count > 0);
        }
    }
}