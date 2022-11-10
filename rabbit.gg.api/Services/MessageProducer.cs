using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace rabbit.gg.api.Services;

public class MessageProducer : IMessageProducer
{
    private readonly ILogger<MessageProducer> _logger;

    public MessageProducer(ILogger<MessageProducer> logger)
    {
        _logger = logger;
    }

    public void PublishMessage<T>(T message)
    {
        using var connection = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "user",
            Password = "password",
            VirtualHost = "/"
        }.CreateConnection();

        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(
            queue: "orders",
            durable: true,
            exclusive: false);
        
        // channel.Queue(
        //     queue: "orders");

        var body = Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(message));
        
        channel.BasicPublish(
            exchange: "",
            routingKey: "orders",
            body: body);
        
        _logger.LogInformation("Message was published");
    }
}
