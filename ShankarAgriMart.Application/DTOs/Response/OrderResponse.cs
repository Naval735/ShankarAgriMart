using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Domain.Enums;

namespace ShankarAgriMart.Application.DTOs.Response;

public class OrderResponse
{
    public int Id { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public int UserId { get; set; }

    public int AddressId { get; set; }

    public decimal SubTotal { get; set; }

    public decimal GST { get; set; }

    public decimal DeliveryCharge { get; set; }

    public decimal Discount { get; set; }

    public decimal GrandTotal { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public PaymentMethod? PaymentMethod { get; set; }

    public DateTime OrderDate { get; set; }

    public List<OrderItemResponse> Items { get; set; } = new();
}