using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Domain.Common;

namespace ShankarAgriMart.Domain.Entities;

public class Crop : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<CropProduct> CropProducts { get; set; }
        = new List<CropProduct>();

    public ICollection<ProductRecommendation> ProductRecommendations { get; set; }
        = new List<ProductRecommendation>();
}
