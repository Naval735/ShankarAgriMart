using ShankarAgriMart.Application.Common.Exceptions;
using ShankarAgriMart.Application.DTOs.Response;
using ShankarAgriMart.Application.Interfaces.Repositories;
using ShankarAgriMart.Application.Interfaces.Services;
using ShankarAgriMart.Domain.Entities;
using ShankarAgriMart.Domain.Enums;
using Microsoft.Extensions.Configuration;
namespace ShankarAgriMart.Application.Services;
using Razorpay.Api;
using PaymentEntity = ShankarAgriMart.Domain.Entities.Payment;
public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IConfiguration _configuration;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        IConfiguration configuration)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _configuration = configuration;
    }

    public async Task<PaymentResponse> CreatePaymentOrderAsync(
        int userId,
        int orderId)
    {
        Console.WriteLine(">>> PAYMENT SERVICE CALLED <<<");
        var order = await _orderRepository
            .GetByIdForUserAsync(orderId, userId);

        if (order == null)
            throw new NotFoundException("Order not found.");

        if (order.PaymentStatus == PaymentStatus.Paid)
            throw new ArgumentException(
                "Order is already paid.");

        if (order.GrandTotal <= 0)
            throw new ArgumentException(
                "Invalid order amount.");

        var existingPayment = await _paymentRepository
            .GetByOrderIdAsync(orderId);

        if (existingPayment != null &&
            !string.IsNullOrEmpty(existingPayment.RazorpayOrderId))
        {
            return new PaymentResponse
            {
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                Amount = order.GrandTotal,
                RazorpayOrderId =
                    existingPayment.RazorpayOrderId,
                Currency = "INR",
                PaymentStatus =
                    existingPayment.PaymentStatus.ToString()
            };
        }

        var keyId = _configuration["Razorpay:KeyId"];
        var keySecret = _configuration["Razorpay:KeySecret"];

        Console.WriteLine($"KeyId loaded: {keyId}");
        Console.WriteLine($"Secret exists: {!string.IsNullOrWhiteSpace(keySecret)}");
        Console.WriteLine($"Secret length: {keySecret?.Length}");

        if (string.IsNullOrWhiteSpace(keyId) ||
            string.IsNullOrWhiteSpace(keySecret))
        {
            throw new InvalidOperationException(
                "Razorpay configuration is missing.");
        }

        var client = new RazorpayClient(keyId, keySecret);

        var options = new Dictionary<string, object>
{
    { "amount", (int)(order.GrandTotal * 100) },
    { "currency", "INR" },
    { "receipt", order.OrderNumber }
};

        var razorpayOrder = client.Order.Create(options);

        var razorpayOrderId =
            razorpayOrder["id"]?.ToString();

        if (string.IsNullOrWhiteSpace(razorpayOrderId))
        {
            throw new InvalidOperationException(
                "Failed to create Razorpay order.");
        }

        var payment = new PaymentEntity
        {
            OrderId = order.Id,
            RazorpayOrderId = razorpayOrderId,
            Amount = order.GrandTotal,
            PaymentMethod = PaymentMethod.Razorpay.ToString(),
            PaymentStatus = PaymentStatus.Pending.ToString()
        };

        await _paymentRepository.CreateAsync(payment);

        return new PaymentResponse
        {
            OrderId = order.Id,
            OrderNumber = order.OrderNumber,
            Amount = order.GrandTotal,
            RazorpayOrderId = razorpayOrderId,
            Currency = "INR",
            PaymentStatus = PaymentStatus.Pending.ToString()
        };
    }

    public async Task<PaymentResponse> VerifyPaymentAsync(
     int userId,
     int orderId,
     string razorpayPaymentId,
     string razorpayOrderId,
     string razorpaySignature)
    {
        var order = await _orderRepository
            .GetByIdForUserAsync(orderId, userId);

        if (order == null)
            throw new NotFoundException("Order not found.");

        if (order.PaymentStatus == PaymentStatus.Paid)
            throw new ArgumentException("Order is already paid.");

        if (string.IsNullOrWhiteSpace(razorpayPaymentId) ||
            string.IsNullOrWhiteSpace(razorpayOrderId) ||
            string.IsNullOrWhiteSpace(razorpaySignature))
        {
            throw new ArgumentException(
                "Payment verification details are required.");
        }

        var payment = await _paymentRepository
            .GetByRazorpayOrderIdAsync(razorpayOrderId);

        if (payment == null || payment.OrderId != orderId)
            throw new NotFoundException("Payment record not found.");

        var keySecret = _configuration["Razorpay:KeySecret"];

        if (string.IsNullOrWhiteSpace(keySecret))
            throw new InvalidOperationException(
                "Razorpay configuration is missing.");

        // Verify Razorpay signature
        var attributes = new Dictionary<string, string>
    {
        { "razorpay_order_id", razorpayOrderId },
        { "razorpay_payment_id", razorpayPaymentId },
        { "razorpay_signature", razorpaySignature }
    };

        Utils.verifyPaymentSignature(attributes);

        // Payment is successfully verified
        payment.RazorpayPaymentId = razorpayPaymentId;
        payment.RazorpaySignature = razorpaySignature;
        payment.PaymentStatus = PaymentStatus.Paid.ToString();
        payment.PaidAt = DateTime.UtcNow;

        order.PaymentStatus = PaymentStatus.Paid;
        order.PaymentMethod = PaymentMethod.Razorpay;
        order.OrderStatus = OrderStatus.Confirmed;

        await _paymentRepository.UpdateAsync(payment);
        await _orderRepository.UpdateAsync(order);

        return new PaymentResponse
        {
            OrderId = order.Id,
            OrderNumber = order.OrderNumber,
            Amount = order.GrandTotal,
            RazorpayOrderId = razorpayOrderId,
            RazorpayPaymentId = razorpayPaymentId,
            Currency = "INR",
            PaymentStatus = PaymentStatus.Paid.ToString()
        };
    }
}