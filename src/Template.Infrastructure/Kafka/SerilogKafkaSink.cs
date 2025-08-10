using Serilog.Core;
using Serilog.Events;

namespace Template.Infrastructure.Kafka
{
    public class SerilogKafkaSink : ILogEventSink
    {
        private readonly IKafkaService _kafkaService;
        private readonly string _topic;

        public SerilogKafkaSink(IKafkaService kafkaService, string topic)
        {
            _kafkaService = kafkaService;
            _topic = topic;
        }

        public void Emit(LogEvent logEvent)
        {
            var message = logEvent.RenderMessage();
            _ = _kafkaService.ProduceAsync(_topic, message);
        }
    }
}
