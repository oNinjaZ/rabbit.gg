namespace rabbit.gg.api.Models;

public class Order
{
    public Guid Id { get; init; }
    public int BookId { get; init; }
    public int UserId { get; init; }
    public DateTime PurchaseDate { get; init; }
    public int Quantity { get; set; }
}
