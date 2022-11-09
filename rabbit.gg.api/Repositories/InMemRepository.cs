using rabbit.gg.api.Models;

namespace rabbit.gg.api.Repositories;

public class InMemRepository
{
    public List<Order> Orders { get; }
    public InMemRepository()
    {
        Orders = new List<Order>
        {
            new() { Id = 1, BookId = 1, UserId = 1, PurchaseDate = DateTime.Now, Quantity = 1 },
            new() { Id = 2, BookId = 2, UserId = 2, PurchaseDate = DateTime.Now, Quantity = 1 },
            new() { Id = 3, BookId = 3, UserId = 3, PurchaseDate = DateTime.Now, Quantity = 1 }
        };
    }
}
