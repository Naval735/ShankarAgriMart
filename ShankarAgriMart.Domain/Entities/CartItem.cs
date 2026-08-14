using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Domain.Common;

namespace ShankarAgriMart.Domain.Entities;

public class CartItem : BaseEntity
{
    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; } = 1;

    public decimal UnitPrice { get; set; }

    public Cart Cart { get; set; } = null!;

    public Product Product { get; set; } = null!;
}