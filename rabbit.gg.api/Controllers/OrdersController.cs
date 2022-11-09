using Microsoft.AspNetCore.Mvc;
using rabbit.gg.api.Dtos;
using rabbit.gg.api.Models;
using rabbit.gg.api.Services;

namespace rabbit.gg.api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IMessageProducer _messageProducer;
    private static readonly List<Order> _orders = new();

    public OrdersController(IMessageProducer messageProducer, ILogger<OrdersController> logger)
    {
        _messageProducer = messageProducer;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
        => Ok(_orders);

    [HttpPost]
    public IActionResult Post(OrderRequest req)
    {
        // if (!ModelState.IsValid)
        //     return BadRequest();

        var order = new Order
        {
            Id = Guid.NewGuid(),
            BookId = req.BookId,
            UserId = req.UserId,
            Quantity = req.Quantity,
            PurchaseDate = DateTime.Now
        };

        _orders.Add(order);
        _logger.LogInformation("Order {OrderId} was created", order.Id);

        _messageProducer.PublishMessage(order);
        return Ok();
    }
}
