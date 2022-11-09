using rabbit.gg.api.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddScoped<IMessageProducer, MessageProducer>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
