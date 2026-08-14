using ShankarAgriMart.Domain.Common;

namespace ShankarAgriMart.Domain.Entities;

public class InventoryTransaction : BaseEntity
{
    public int ProductId { get; set; }

    public string TransactionType { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public string? Remarks { get; set; }

    public Product Product { get; set; } = null!;
}