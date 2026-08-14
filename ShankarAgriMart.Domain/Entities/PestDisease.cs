using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShankarAgriMart.Domain.Common;

namespace ShankarAgriMart.Domain.Entities;

public class PestDisease : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Type { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<ProductRecommendation> ProductRecommendations { get; set; }
        = new List<ProductRecommendation>();
}