using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Infrastructure.Kafka
{
    public class KafkaConsumerHostedService : BackgroundService
    {
        private readonly IKafkaService _kafkaService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<KafkaConsumerHostedService> _logger;

        public KafkaConsumerHostedService(
            IKafkaService kafkaService,
            IConfiguration configuration,
            ILogger<KafkaConsumerHostedService> logger)
        {
            _kafkaService = kafkaService;
            _configuration = configuration;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var topic = _configuration["Kafka:Topic"];
            _logger.LogInformation("KafkaConsumerHostedService started for topic: {Topic}", topic);
            try
            {
                await _kafkaService.ConsumeAsync(topic, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Kafka consumer stopped.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kafka consumer failed.");
            }
        }
    }

}
