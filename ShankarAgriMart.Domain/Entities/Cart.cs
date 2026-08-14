using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Domain.Common;

namespace ShankarAgriMart.Domain.Entities;

public class Cart : BaseEntity
{
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public ICollection<CartItem> CartItems { get; set; }
        = new List<CartItem>();
}