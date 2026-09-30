using Azure.Monitor.OpenTelemetry.Exporter;
using Capium.Bookkeeping.AutoaccountingPeriodFunction;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Configuration;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Exceptions;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.LoggingGateway;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Mappers;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Repositories;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Services;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using System.Reflection;

var host = new HostBuilder()
    .ConfigureAppConfiguration((_, configurationBuilder) =>
    {
        var environment = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") ?? "Production";

        configurationBuilder
            .AddCommandLine(args)
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile($"{environment}.settings.json", optional: true, reloadOnChange: true)
            .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
            .AddEnvironmentVariables();

        configurationBuilder.Build().AddAzureKeyVaultConfig(configurationBuilder);
    })
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((HostBuilderContext context, IServiceCollection services) =>
    {
        services.AddLogging();
        services.AddHttpClient();
        services.AddHttpContextAccessor();

        services.Configure<MySettings>(context.Configuration.GetSection("MySettings"));
        services.AddOptions<LoggingGatewayOptions>()
            .Configure(options => context.Configuration.GetSection(LoggingGatewayOptions.SectionName).Bind(options))
            .PostConfigure(options =>
            {
                options.BaseUrl = context.Configuration["LoggingFrameWorkAPIBaseUrl"] ?? options.BaseUrl;

                if (string.IsNullOrWhiteSpace(options.Audience)
                    || string.IsNullOrWhiteSpace(options.BaseUrl))
                {
                    context.Configuration.GetSection(LoggingGatewayOptions.LegacySectionName).Bind(options);
                }
            });

        services.AddHttpClient<ILoggingGatewayClient, LoggingGatewayClient>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<LoggingGatewayOptions>>().Value;
            if (!Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out var baseUri))
            {
                throw new InvalidOperationException(
                    "Logging Gateway BaseUrl is not configured. Configure LoggingFrameWorkAPIBaseUrl or LoggingGateway:BaseUrl.");
            }

            client.BaseAddress = baseUri;
        });

        services.AddSingleton<AzureKeyVaultValues>();
        services.AddSingleton<MyConfiguration>();
        services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
        services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        services.AddScoped<DapperDBContext>();
        services.AddScoped<IAccountingPeriodRepository, AccountingPeriodRepository>();
        services.AddScoped<IClientDetailsRepository, ClientDetailsRepository>();
        services.AddScoped<ILoggingFrameworkService, LoggingFrameworkService>();
        services.AddScoped<ICommunicationFrameworkService, CommunicationFrameworkService>();
        services.AddScoped<IEmailTemplateService, EmailTemplateService>();
        services.AddScoped<ITemplateGenerator, TemplateGenerator>();
        services.AddScoped<ISendGridEmailService, SendGridEmailService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAutoaccountingPeriodService, AutoaccountingPeriodService>();
        services.AddScoped<IExceptionLog, ExceptionLog>();

        var appInsightsConnectionString = context.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
        if (!string.IsNullOrEmpty(appInsightsConnectionString))
        {
            services.AddOpenTelemetry()
                .UseFunctionsWorkerDefaults()
                .UseAzureMonitorExporter();
        }
    })
    .Build();

host.Run();
