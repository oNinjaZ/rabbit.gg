using System.Text;
using System.Text.Json;
using rabbit.gg.orders.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using rabbit.gg.orders.Services;
using Microsoft.Extensions.DependencyInjection;
using rabbit.gg.orders.Data;

using var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        Console.WriteLine($"Environment: {envName}");

        config.AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile(path: $"appsettings.{envName ?? "Production"}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config.Build())
            .CreateLogger();
    })
    .UseSerilog()
    .ConfigureServices((ctx, services) =>
    {
        services.AddSingleton<EmailService>();
        services.AddSingleton<IMQConnectionFactory>(_ => new RMQConnectionFactory(
            ctx.Configuration.GetValue<string>("RabbitMQ:HostName")!, 
            ctx.Configuration.GetValue<string>("RabbitMQ:UserName")!, 
            ctx.Configuration.GetValue<string>("RabbitMQ:Password")!));
    })
    .Build();

var connection = host.Services.GetRequiredService<IMQConnectionFactory>().
    CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "orders",
    durable: true,
    exclusive: false);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var order = JsonSerializer.Deserialize<OrderRequest>(message)!;
    Console.WriteLine(
        $@"Received message!
        OrderId: {order.Id}
        BookId: {order.BookId}
        UserId: {order.UserId}
        Quantity: {order.Quantity}
        PurchaseDate: {order.PurchaseDate}");

    // acknowledge message
    channel.BasicAck(ea.DeliveryTag, false);
};

channel.BasicConsume(
    queue: "orders",
    autoAck: false,
    consumer: consumer);

await host.RunAsync();
