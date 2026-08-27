using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.Interfaces.Services;

namespace ShankarAgriMart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Customer")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // POST: api/Order
    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderRequest request)
    {
        var userId = GetUserId();

        var order = await _orderService.CreateOrderAsync(
            userId,
            request);

        return Ok(order);
    }

    // GET: api/Order
    [HttpGet]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = GetUserId();

        var orders = await _orderService
            .GetMyOrdersAsync(userId);

        return Ok(orders);
    }

    // GET: api/Order/5
    [HttpGet("{orderId:int}")]
    public async Task<IActionResult> GetOrder(
        int orderId)
    {
        var userId = GetUserId();

        var order = await _orderService
            .GetOrderByIdAsync(userId, orderId);

        return Ok(order);
    }

    // POST: api/Order/cancel/5
    [HttpPost("cancel/{orderId:int}")]
    public async Task<IActionResult> CancelOrder(int orderId)
    {
        var userId = GetUserId();

        var order = await _orderService.CancelOrderAsync(
            userId,
            orderId);

        return Ok(order);
    }
    private int GetUserId()
    {
        var userIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException(
                "User ID was not found in the token.");

        return userId;
    }
}