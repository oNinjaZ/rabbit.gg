using Microsoft.Extensions.Logging;
using rabbit.gg.orders.Contracts;

namespace rabbit.gg.orders.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    public Guid EmailId { get; } = Guid.NewGuid(); 

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(OrderRequest order)
    {
        await Task.Delay(1000);

        var newGuid = Guid.NewGuid();
        _logger.LogInformation(
            $@"Sending email!
            ServiceId FromConstructor: {EmailId}
            ServiceId FromMethod: {newGuid}
            OrderId: {order.Id}
            BookId: {order.BookId}
            UserId: {order.UserId}
            Quantity: {order.Quantity}
            PurchaseDate: {order.PurchaseDate}");

        return true;
    }
}
