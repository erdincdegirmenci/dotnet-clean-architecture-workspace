using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace Template.Infrastructure.Kafka;

public class KafkaService : IKafkaService
{
    private readonly IConfiguration _configuration;
    private readonly string _bootstrapServers;

    public KafkaService(IConfiguration configuration)
    {
        _configuration = configuration;
        _bootstrapServers = _configuration["Kafka:BootstrapServers"]!;
    }

    public async Task ProduceAsync(string topic, string message)
    {
        var config = new ProducerConfig { BootstrapServers = _bootstrapServers };
        using var producer = new ProducerBuilder<Null, string>(config).Build();
        await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
    }

    public async Task ConsumeAsync(string topic, CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = "dotnet-template-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(topic);
        while (!cancellationToken.IsCancellationRequested)
        {
            var result = consumer.Consume(cancellationToken);
            // Burada mesajı işleyebilirsin
            Console.WriteLine($"Kafka message: {result.Message.Value}");
        }
    }
} 