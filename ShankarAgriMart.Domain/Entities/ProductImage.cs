using ShankarAgriMart.Domain.Common;

namespace ShankarAgriMart.Domain.Entities;

public class ProductImage : BaseEntity
{
    public int ProductId { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public bool IsPrimary { get; set; }

    public int DisplayOrder { get; set; }

    public Product Product { get; set; } = null!;
}