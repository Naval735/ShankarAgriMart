using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShankarAgriMart.Domain.Common;
using ShankarAgriMart.Domain.Enums;

namespace ShankarAgriMart.Domain.Entities;

public class Order : BaseEntity
{
    public string OrderNumber { get; set; } = string.Empty;

    public int UserId { get; set; }

    public int AddressId { get; set; }

    public decimal SubTotal { get; set; }

    public decimal GST { get; set; }

    public decimal DeliveryCharge { get; set; }

    public decimal Discount { get; set; }

    public decimal GrandTotal { get; set; }

    public PaymentStatus PaymentStatus { get; set; }
        = PaymentStatus.Pending;

    public OrderStatus OrderStatus { get; set; }
        = OrderStatus.Placed;

    public PaymentMethod? PaymentMethod { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;


    // Navigation properties

    public User User { get; set; } = null!;

    public Address Address { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; }
        = new List<OrderItem>();

    public Payment? Payment { get; set; }
}
