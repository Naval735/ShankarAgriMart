using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShankarAgriMart.Application.DTOs.Request;

public class CreateBrandRequest
{
    public string Name { get; set; } = string.Empty;

    public string? LogoUrl { get; set; }

    public string? Description { get; set; }
}