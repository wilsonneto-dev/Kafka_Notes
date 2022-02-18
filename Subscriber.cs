using Confluent.Kafka;

namespace BooksCatalog.Api;

public class Subscriber : BackgroundService
{
    public string TopicName { get; set; } = "books";
    public string KafkaServer { get; set; } = "localhost:9092";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = KafkaServer,
            GroupId = $"processor-group-0",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        CancellationTokenSource cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        try
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(TopicName);

                try
                {
                    while (true)
                    {
                        var cr = consumer.Consume(cts.Token);
                        Console.WriteLine($"Mensagem lida: {cr.Message.Value}");
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                    Console.WriteLine("Cancelada a execução do Consumer...");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Exceção: {ex.GetType().FullName} | " +
                $"Mensagem: {ex.Message}"
            );
        }

   }
}
