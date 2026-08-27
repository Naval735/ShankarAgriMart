using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShankarAgriMart.Application.DTOs.Response;

public class CartResponse
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public List<CartItemResponse> Items { get; set; } = new();

    public decimal TotalAmount { get; set; }
}