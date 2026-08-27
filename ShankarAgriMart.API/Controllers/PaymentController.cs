using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.Interfaces.Services;

namespace ShankarAgriMart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("create/{orderId}")]
    public async Task<IActionResult> CreatePaymentOrder(int orderId)
    {
        var userIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out int userId))
            return Unauthorized();

        var result = await _paymentService
            .CreatePaymentOrderAsync(userId, orderId);

        return Ok(new
        {
            Success = true,
            Message = "Razorpay payment order created successfully.",
            Data = result
        });
    }
    [HttpPost("verify/{orderId}")]
    public async Task<IActionResult> VerifyPayment(
    int orderId,
    [FromBody] VerifyPaymentRequest request)
    {
        var userIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out int userId))
            return Unauthorized();

        var result = await _paymentService
            .VerifyPaymentAsync(
                userId,
                orderId,
                request.RazorpayPaymentId,
                request.RazorpayOrderId,
                request.RazorpaySignature);

        return Ok(new
        {
            Success = true,
            Message = "Payment verified successfully.",
            Data = result
        });
    }
}