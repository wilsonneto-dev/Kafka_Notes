using Confluent.Kafka;
using Newtonsoft.Json;

namespace BooksCatalog.Api.Integration;

public class KafkaEventBus : IEventBus
{
    public string TopicName { get; set; } = "books";
    public string KafkaServer { get; set; } = "localhost:9092";


    public async Task Publish(object obj)
    {
        try
        {
            var config = new ProducerConfig
            {
                BootstrapServers = KafkaServer
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                var result = await producer.ProduceAsync(
                    TopicName,
                    new Message<Null, string>()
                        { Value = JsonConvert.SerializeObject(obj) }
                );

                Console.WriteLine(
                    $"Mensagem: {JsonConvert.SerializeObject(obj)} | " +
                    $"Status: { result.Status.ToString()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Exceção: {ex.GetType().FullName} | " +
                $"Mensagem: {ex.Message}");
        }
    }
}
