using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ShankarAgriMart.Application.DTOs.Request;

public class CreateProductRequest
{
    public int CategoryId { get; set; }

    public int BrandId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public decimal MRP { get; set; }

    public decimal SellingPrice { get; set; }

    public decimal GST { get; set; } = 18;

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
}