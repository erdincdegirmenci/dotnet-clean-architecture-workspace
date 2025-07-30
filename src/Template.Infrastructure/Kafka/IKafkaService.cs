namespace Template.Infrastructure.Kafka;

public interface IKafkaService
{
    Task ProduceAsync(string topic, string message);
    Task ConsumeAsync(string topic, CancellationToken cancellationToken);
} 