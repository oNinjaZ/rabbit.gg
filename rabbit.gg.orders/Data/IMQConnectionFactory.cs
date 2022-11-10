using RabbitMQ.Client;

namespace rabbit.gg.orders.Data;

public interface IMQConnectionFactory
{
    IConnection CreateConnection();
}
