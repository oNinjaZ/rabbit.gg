namespace rabbit.gg.orders.Contracts;

public record OrderRequest(
    Guid Id,
    int BookId,
    int UserId,
    int Quantity,
    DateTime PurchaseDate);
