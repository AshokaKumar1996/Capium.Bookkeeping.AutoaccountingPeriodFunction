using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.AccountingPeriod;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface
{
    public interface IAccountingPeriodRepository
    {
        Task<AccountingPeriodModel?> GetLatestByCompanyIdAsync(Guid companyId);

        Task<Guid> CreateAsync(AccountingPeriodModel model);
    }
}
