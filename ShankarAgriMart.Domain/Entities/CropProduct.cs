using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Domain.Common;

namespace ShankarAgriMart.Domain.Entities;

public class CropProduct : BaseEntity
{
    public int CropId { get; set; }

    public int ProductId { get; set; }

    public Crop Crop { get; set; } = null!;

    public Product Product { get; set; } = null!;
}