using System;
using System.Collections.Generic;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.Email
{
    public class EmailModel
    {
        public EmailModel()
        {
            Attachments = new List<EmailAttachmentModel>();
            EmailContent = new EmailContentModel();
            ToEmail = new List<string>();
            ModulesRequest = new ModuleRequestModel();
        }

        public string FromEmail { get; set; } = string.Empty;

        public List<string> ToEmail { get; set; }

        public string Subject { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string FromName { get; set; } = string.Empty;

        public string ToName { get; set; } = string.Empty;

        public string TemplateName { get; set; } = string.Empty;

        public bool IsSendFromCapium { get; set; }

        public Guid ProviderAccountId { get; set; }

        public long ProviderId { get; set; }

        public decimal Amount { get; set; }

        public EmailContentModel EmailContent { get; set; }

        public List<EmailAttachmentModel> Attachments { get; set; }

        public ModuleRequestModel ModulesRequest { get; set; }

        public Guid UserId { get; internal set; }
    }

    public class ModuleRequestModel
    {
        public int ModuleId { get; set; }

        public string CompanyId { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
    }

    public class EmailContentModel
    {
        public string Content { get; set; } = string.Empty;
    }

    public class EmailAttachmentModel
    {
        public string FileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;
    }

    public class ExecutionReportModel
    {
        public int NumberOfRecordsFound { get; set; }

        public int NumberOfRecordsUpdated { get; set; }

        public int NumberOfRecordsCreated { get; set; }

        public int NumberOfRecordsDeleted { get; set; }

        public string AzureFunctionName { get; set; } = string.Empty;

        public string EnvironmentName { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime ExecutionDate { get; set; }
    }
}
