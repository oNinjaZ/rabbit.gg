using Microsoft.AspNetCore.Mvc;
using rabbit.gg.api.Services;

namespace rabbit.gg.api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMessageProducer _messageProducer;

    public AuthController(IMessageProducer messageProducer)
    {
        _messageProducer = messageProducer;
    }

    [HttpPost]
    public IActionResult Register()
    {
        // register user

        // publish message to email confirmation service
        _messageProducer.PublishMessage(new
        {
            Message = "Send email to ---"
        });

        return Ok();
    }
}
