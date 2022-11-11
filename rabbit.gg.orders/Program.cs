using System.Text;
using System.Text.Json;
using rabbit.gg.orders.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using rabbit.gg.orders.Services;
using rabbit.gg.orders.Data;

var builder = WebApplication.CreateBuilder(args);
{
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);

    builder.Services.AddSingleton<IMQConnectionFactory>(_ => new RMQConnectionFactory(
        builder.Configuration.GetValue<string>("RabbitMQ:HostName")!,
        builder.Configuration.GetValue<string>("RabbitMQ:UserName")!,
        builder.Configuration.GetValue<string>("RabbitMQ:Password")!));

    builder.Services.AddSingleton<IEmailService, EmailService>();

    builder.Services.AddHealthChecks();
}

var app = builder.Build();

app.MapHealthChecks("/health");

var connection = app.Services.GetRequiredService<IMQConnectionFactory>().
    CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "orders",
    durable: true,
    exclusive: false);

var consumer = new EventingBasicConsumer(channel);

channel.BasicConsume(
    queue: "orders",
    autoAck: false,
    consumer: consumer);

var emailService = app.Services.GetRequiredService<IEmailService>();

consumer.Received += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var order = JsonSerializer.Deserialize<OrderRequest>(message)!;

    if (await emailService.SendEmailAsync(order))
        channel.BasicAck(ea.DeliveryTag, false);
};

app.Run();
