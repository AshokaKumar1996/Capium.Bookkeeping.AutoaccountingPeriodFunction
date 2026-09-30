using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.Client;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface
{
    public interface IClientDetailsRepository
    {
        Task<List<ClientDetailModel>> GetAutoAccountingPeriodClientsAsync();
    }
}
