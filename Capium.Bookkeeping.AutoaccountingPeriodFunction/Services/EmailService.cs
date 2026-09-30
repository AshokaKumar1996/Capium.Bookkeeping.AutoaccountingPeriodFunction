using Capium.Bookkeeping.AutoaccountingPeriodFunction.Configuration;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Enums;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.Email;
using RestSharp;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Services
{
    public class EmailService : IEmailService
    {
        private readonly ISendGridEmailService _sendGridEmailService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly ITemplateGenerator _templateGenerator;
        private readonly ICommunicationFrameworkService _communicationFrameworkService;
        private readonly MySettings _mySettings;

        public EmailService(
            ISendGridEmailService sendGridEmailService,
            IEmailTemplateService emailTemplateService,
            ITemplateGenerator templateGenerator,
            ICommunicationFrameworkService communicationFrameworkService,
            MyConfiguration myConfiguration)
        {
            _sendGridEmailService = sendGridEmailService;
            _emailTemplateService = emailTemplateService;
            _templateGenerator = templateGenerator;
            _communicationFrameworkService = communicationFrameworkService;
            _mySettings = myConfiguration.Settings;
        }

        public async Task<bool> SendEmailAsync<TData>(TData data, Guid companyId) where TData : EmailModel
        {
            var templateDetail = await _emailTemplateService.GetEmailTemplateAsync(
                data.TemplateName,
                companyId,
                data);

            if (templateDetail is not null)
            {
                data.Message = templateDetail.TemplateHTML;
                data.Subject = templateDetail.Subject;

                data.Message = _templateGenerator.GetEmailTemplate(templateDetail.BannerImage, data.Message);

                return await _sendGridEmailService.SendEmailAsync(data, companyId);
            }

            return false;
        }

        public async Task<bool> SendReminderEmailAsync(EmailModel emailModel, long businessId)
        {
            return await SendAsync(emailModel, businessId);
        }
        public async Task<bool> SendAsync(EmailModel emailModel, long businessId)
        {
            emailModel.Message = emailModel.Message.Replace("£", "&#8356;");
            emailModel.FromEmail = _mySettings.AzureKeyVaultValues.SenderName;
            emailModel.FromName = _mySettings.AzureKeyVaultValues.SenderName;

            emailModel.ModulesRequest = new ModuleRequestModel()
            {
                ModuleId = (int)StorageModule.Storage,
                CompanyId = Convert.ToString(businessId),
                UserId = Convert.ToString(emailModel.UserId)
            };

            RestResponse result = await _communicationFrameworkService.SendEmailFromCommunicationAsync(emailModel);
            return result.IsSuccessful;
        }
    }
}