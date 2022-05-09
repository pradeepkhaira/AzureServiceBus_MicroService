using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using BurgerKingOrderProcess.MicroService.Data;
using BurgerKingOrderProcess.MicroService.Models;

namespace BurgerKingOrderProcess.MicroService.Events
{
    public class ServiceBusTopicSubscription : IServiceBusTopicSubscription
    {
        private readonly IProcessData _processData;
        private readonly IConfiguration _configuration;
        private const string _topic = "burgertopic";
        private const string _subcription = "burgerksubscriptions";
        private readonly ILogger _logger;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusAdministrationClient _adminClient;
        private ServiceBusProcessor _processor;

        public ServiceBusTopicSubscription(IProcessData processData, IConfiguration configuration, ILogger<ServiceBusTopicSubscription> logger)
        {
            _processData = processData;
            _configuration = configuration;
            _logger = logger;
            var connectionString = _configuration["AzureServiceBus:BKServiceBus"];
            _client = new ServiceBusClient(connectionString);
            _adminClient = new ServiceBusAdministrationClient(connectionString);
        }

        public async Task PrepareFiltersAndHandleMessages()
        {
            ServiceBusProcessorOptions _serviceBusProcessorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,
            };

            _processor = _client.CreateProcessor(_topic, _subcription, _serviceBusProcessorOptions);
            _processor.ProcessMessageAsync += ProcessMessagesAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;

            await RemoveDefaultFilters();
            await AddFilters();

            await _processor.StartProcessingAsync().ConfigureAwait(false);
        }

        private async Task RemoveDefaultFilters()
        {
            try
            {
                var rules = _adminClient.GetRulesAsync(_topic, _subcription);
                var ruleProperties = new List<RuleProperties>();
                await foreach (var rule in rules)
                {
                    ruleProperties.Add(rule);
                }

                foreach (var rule in ruleProperties)
                {
                    if (rule.Name == "OrderGreaterThanTwo")
                    {
                        await _adminClient.DeleteRuleAsync(_topic, _subcription, "OrderGreaterThanTwo")
                            .ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
            }
        }

        private async Task AddFilters()
        {
            try
            {
                var rules = _adminClient.GetRulesAsync(_topic, _subcription)
                    .ConfigureAwait(false);

                var ruleProperties = new List<RuleProperties>();
                await foreach (var rule in rules)
                {
                    ruleProperties.Add(rule);
                }

                if (!ruleProperties.Any(r => r.Name == "OrderGreaterThanTwo"))
                {
                    CreateRuleOptions createRuleOptions = new CreateRuleOptions
                    {
                        Name = "OrderGreaterThanTwo",
                        Filter = new SqlRuleFilter("order > 2")
                    };
                    await _adminClient.CreateRuleAsync(_topic, _subcription, createRuleOptions)
                        .ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
            }
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            var myPayload = args.Message.Body.ToObjectFromJson<BurgerOrder>();
            await _processData.Process(myPayload).ConfigureAwait(false);
            await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            _logger.LogError(arg.Exception, "Message handler encountered an exception");
            _logger.LogDebug($"- ErrorSource: {arg.ErrorSource}");
            _logger.LogDebug($"- Entity Path: {arg.EntityPath}");
            _logger.LogDebug($"- FullyQualifiedNamespace: {arg.FullyQualifiedNamespace}");

            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (_processor != null)
            {
                await _processor.DisposeAsync().ConfigureAwait(false);
            }

            if (_client != null)
            {
                await _client.DisposeAsync().ConfigureAwait(false);
            }
        }

        public async Task CloseQueueAsync()
        {
            await _processor.CloseAsync().ConfigureAwait(false);
        }
    }
}
