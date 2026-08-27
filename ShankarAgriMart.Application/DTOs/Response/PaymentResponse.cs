using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShankarAgriMart.Application.DTOs.Response;

public class PaymentResponse
{
    public int OrderId { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string RazorpayOrderId { get; set; } = string.Empty;

    public string Currency { get; set; } = "INR";

    public string? RazorpayPaymentId { get; set; }

    public string PaymentStatus { get; set; } = string.Empty;
}