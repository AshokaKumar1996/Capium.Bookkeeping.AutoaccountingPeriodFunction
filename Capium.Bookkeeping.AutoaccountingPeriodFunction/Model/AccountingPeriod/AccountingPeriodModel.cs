namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.AccountingPeriod
{
    public class AccountingPeriodModel
    {
        public Guid Id { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public Guid CompanyId { get; set; }

        public bool IsLocked { get; set; }

        public bool IsCeased { get; set; }

        public bool IsComparative { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}
