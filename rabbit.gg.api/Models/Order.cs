namespace rabbit.gg.api.Models;

public class Order
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int Quantity { get; set; }
}
