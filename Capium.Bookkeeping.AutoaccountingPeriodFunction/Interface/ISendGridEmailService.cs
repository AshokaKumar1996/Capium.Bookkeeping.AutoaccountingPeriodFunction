using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.Email;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface
{
    public interface ISendGridEmailService
    {
        Task<bool> SendEmailAsync(EmailModel emailModel, Guid companyId);
    }
}