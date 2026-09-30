using Capium.Bookkeeping.AutoaccountingPeriodFunction.Configuration;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Constants;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.Email;
using System.Text;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly MySettings _mySettings;

        public EmailTemplateService(MyConfiguration myConfiguration)
        {
            _mySettings = myConfiguration.Settings;
        }

        public string GetReminderEmailTemplateWithHeaderAndFooter(
            string status,
            string content,
            ExecutionReportModel executionReportModel)
        {
            string url = $"{_mySettings.AzureKeyVaultValues.AzureCDNUrl}{CdnFolder.Bookkeeping}/";
            string env = $"{Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT")}/";
            string logoImage = $"{url}{CdnFolder.LogoImage}";

            StringBuilder receipt = new();
            if (status == "Success")
            {
                receipt.Append($@"<!DOCTYPE html>");
                receipt.Append($@"<html lang='en'>");
                receipt.Append($@"<head>");
                receipt.Append($@"<title>Execution Report</title>");
                receipt.Append($@"</head>");
                receipt.Append($@"<body>");
                receipt.Append($@"<table cellspacing='0' cellpadding='5' style='font-family: calibri; background: #F7F7F7;'>");
                receipt.Append($@"<tr>");
                receipt.Append($@"<td style='background: #F5EBFF;padding: 6px 8px;text-align: right;height: 56px;'>");
                receipt.Append($@"<img src='{logoImage}' alt='logo' width='100' height='45' /></td>");
                receipt.Append($@"</tr>");
                receipt.Append($@"<tr>");
                receipt.Append($@"<td style='padding: 25px 10px 0 10px;display:block;'>");
                receipt.Append($@"<h5 style = 'color: #624096;margin-top:0px; margin-bottom:20px;font-size: 16px;' >");
                receipt.Append($@"<p style='line-height: 24px;'>Dear Team <br/> <br/>This is to inform you that the <b>Bookkeeping</b> module triggered <br/><b>func-AutoaccountingPeriod</b> in the <b>" + env + "</b> environment on <b>" + DateTime.Now + "</b> with a start time of <b>" + executionReportModel.StartTime + "</b>, ending at <b>" + executionReportModel.EndTime + "</b>.</p>");
                receipt.Append($@"<p style='line - height: 24px;'>The Azure function<b>func-AutoaccountingPeriod</b> has completed its execution.<br/> During this process, the following changes were made:</p> ");
                receipt.Append($@"<ul type='none' style='padding-left: 10px;line-height: 24px;'>");
                receipt.Append($@"<li>- Number of records found: <b>" + executionReportModel.NumberOfRecordsFound + "</b></li>");
                receipt.Append($@"<li>- Number of records created: <b>" + executionReportModel.NumberOfRecordsCreated + "</b></li>");
                receipt.Append($@"<li>- Number of records updated: <b>" + executionReportModel.NumberOfRecordsUpdated + "</b></li>");
                receipt.Append($@"</ul>");
                receipt.Append($@"</td>");
                receipt.Append($@"</tr>");
                receipt.Append($@"<tr>");
                receipt.Append($@"<td style='background: #624096;color: #ffffff;text-align: center;font-size: 12px;padding: 0px;'>");
                receipt.Append($@"<p>Copyrights Company All Rights Reserved</p>");
                receipt.Append($@"</td>");
                receipt.Append($@"</tr>");
                receipt.Append($@"</table>");
                receipt.Append($@"</body>");
                receipt.Append($@"</html>");
            }
            else
            {
                receipt.Append($@"<!DOCTYPE html>");
                receipt.Append($@"<html lang='en'>");
                receipt.Append($@"<head>");
                receipt.Append($@"<title>Execution Report</title>");
                receipt.Append($@"</head>");
                receipt.Append($@"<body>");
                receipt.Append($@"<table cellspacing='0' cellpadding='5' style='font-family: calibri; background: #F7F7F7;'>");
                receipt.Append($@"<tr>");
                receipt.Append($@"<td style='background: #F5EBFF;padding: 6px 8px;text-align: right;height: 56px;'>");
                receipt.Append($@"<img src='{logoImage}' alt='logo' width='100' height='45' /></td>");
                receipt.Append($@"</tr>");
                receipt.Append($@"<tr>");
                receipt.Append($@"<td style='padding: 25px 10px 0 10px;display:block;'>");
                receipt.Append($@"<h5 style = 'color: #624096;margin-top:0px; margin-bottom:20px;font-size: 16px;' >");
                receipt.Append($@"<p style='line-height: 24px;'>Dear Team <br/> <br/>This is to inform you that the <b>Bookkeeping</b> module triggered <br/><b>func-AutoaccountingPeriod</b> in the <b>" + env + "</b> environment on <b>" + DateTime.Now + "</b> with a start time of <b>" + executionReportModel.StartTime + "</b>.</p>");
                receipt.Append($@"<p style='line - height: 24px;'>The Azure function<b>func-AutoaccountingPeriod</b> has started its execution.<br/> During this process, the following exception occur:" + content + " at time <b>" + executionReportModel.EndTime + "</b> </p> ");
                receipt.Append($@"</td>");
                receipt.Append($@"</tr>");
                receipt.Append($@"<tr>");
                receipt.Append($@"<td style='background: #624096;color: #ffffff;text-align: center;font-size: 12px;padding: 0px;'>");
                receipt.Append($@"<p>Copyrights Company All Rights Reserved</p>");
                receipt.Append($@"</td>");
                receipt.Append($@"</tr>");
                receipt.Append($@"</table>");
                receipt.Append($@"</body>");
                receipt.Append($@"</html>");
            }

            return receipt.ToString();
        }

        public async Task<TemplateDetailModel> GetEmailTemplateAsync<TData>(
            string templateName,
            Guid companyId,
            TData data) where TData : EmailModel
        {
            var model = new TemplateDetailModel
            {
                TemplateHTML = data.Message,
                Subject = data.Subject,
                BannerImage = "logo.png"
            };

            return await Task.FromResult(model);
        }
    }
}