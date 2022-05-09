using Azure.Messaging.ServiceBus;
using BurgerKingOrder.MicroService.Controllers;
using System.Text.Json;

namespace BurgerKingOrder.MicroService.Topic
{
    public class ServiceBusTopicSender
    {
        private readonly string _topic;
        private readonly string connectionString;
        private readonly ILogger _logger;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _clientSender;
        private readonly IConfiguration _configuration;

        public ServiceBusTopicSender(IConfiguration configuration, ILogger<ServiceBusTopicSender> logger)
        {
            _logger = logger;
            _configuration = configuration;
            connectionString = _configuration["ConnectionStrings:BKServiceBus"];
            _topic = _configuration["ConnectionStrings:SBTopic"];
            _client = new ServiceBusClient(connectionString);
            _clientSender = _client.CreateSender(_topic);
        }

        public async Task SendMessage(BurgerOrder model)
        {
            string messagePayload = JsonSerializer.Serialize(model);
            ServiceBusMessage message = new ServiceBusMessage(messagePayload);

            message.ApplicationProperties.Add("order",model.OrderName);

            try
            {
                await _clientSender.SendMessageAsync(message).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}
