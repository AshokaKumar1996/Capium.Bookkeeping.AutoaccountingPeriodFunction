namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.Client
{
    public class ClientDetailModel
    {
        public int Id { get; set; }

        public Guid? UniversalId { get; set; }

        public Guid? ClientId { get; set; }

        public bool? IsBranchDepartmentAccounting { get; set; }

        public int? AccountingMethodId { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public long? LegacyClientId { get; set; }

        public string? Logo { get; set; }

        public byte TemplateType { get; set; }

        public bool IsAutoAccountingPeriod { get; set; }
    }
}
