namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface
{
    public interface ITemplateGenerator
    {
        string GetEmailTemplate(string bannerImage, string message);
    }
}