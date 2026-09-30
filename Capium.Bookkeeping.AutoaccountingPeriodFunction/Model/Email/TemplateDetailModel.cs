namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.Email
{
    public class TemplateDetailModel
    {
        public Guid CompanyTemplateId { get; set; }
        public int EmailTemplateId { get; set; }
        public string DisplayName { get; set; }
        public string Subject { get; set; }
        public string TemplateHTML { get; set; }
        public string TemplateParams { get; set; }
        public object SuggestedLabels { get; set; }
        public string BannerImage { get; set; }
        public Guid CompanyId { get; set; }
    }
}