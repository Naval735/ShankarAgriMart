using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShankarAgriMart.Application.DTOs.Request;

public class ProductFilterRequest
{
    public string? Search { get; set; }

    public int? CategoryId { get; set; }

    public int? BrandId { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public bool? IsFeatured { get; set; }

    public string? SortBy { get; set; }

    public bool SortDescending { get; set; } = false;

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 12;
}