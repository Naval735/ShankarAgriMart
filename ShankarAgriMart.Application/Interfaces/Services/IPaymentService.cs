using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Application.DTOs.Response;

namespace ShankarAgriMart.Application.Interfaces.Services;

public interface IPaymentService
{
    Task<PaymentResponse> CreatePaymentOrderAsync(
        int userId,
        int orderId);

    Task<PaymentResponse> VerifyPaymentAsync(
        int userId,
        int orderId,
        string razorpayPaymentId,
        string razorpayOrderId,
        string razorpaySignature);
}