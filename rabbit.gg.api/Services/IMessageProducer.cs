namespace rabbit.gg.api.Services;

public interface IMessageProducer
{
    void PublishMessage<T>(T message);
}
