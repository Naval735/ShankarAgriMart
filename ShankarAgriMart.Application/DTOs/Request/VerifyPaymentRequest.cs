using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShankarAgriMart.Application.DTOs.Request;

public class VerifyPaymentRequest
{
    public string RazorpayPaymentId { get; set; } = string.Empty;

    public string RazorpayOrderId { get; set; } = string.Empty;

    public string RazorpaySignature { get; set; } = string.Empty;
}
