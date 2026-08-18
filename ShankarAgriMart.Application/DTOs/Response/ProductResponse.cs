using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShankarAgriMart.Application.DTOs.Response;

public class ProductResponse
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public int BrandId { get; set; }

    public string BrandName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty;

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public decimal MRP { get; set; }

    public decimal SellingPrice { get; set; }

    public decimal GST { get; set; }

    public int Stock { get; set; }

    public decimal? Weight { get; set; }

    public string? Unit { get; set; }

    public string? ActiveIngredient { get; set; }

    public string? Dosage { get; set; }

    public string? ApplicationMethod { get; set; }

    public string? Benefits { get; set; }

    public string? UsageInstructions { get; set; }

    public string? SafetyPrecautions { get; set; }

    public string? Manufacturer { get; set; }

    public string? CountryOfOrigin { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsActive { get; set; }

    public List<string> Images { get; set; } = new();
}