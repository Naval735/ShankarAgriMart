using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Domain.Common;

namespace ShankarAgriMart.Domain.Entities;

public class Payment : BaseEntity
{
    public int OrderId { get; set; }

    public string? RazorpayOrderId { get; set; }

    public string? RazorpayPaymentId { get; set; }

    public string? RazorpaySignature { get; set; }

    public decimal Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime? PaidAt { get; set; }


    public Order Order { get; set; } = null!;
}