using rabbit.gg.orders.Contracts;

namespace rabbit.gg.orders.Services;

public interface IEmailService
{
    Task<bool> SendEmailAsync (OrderRequest order);
}
