using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ShankarAgriMart.Application.Interfaces.Services;
using ShankarAgriMart.Domain.Enums;

namespace ShankarAgriMart.API.Controllers;

[ApiController]
[Route("api/admin/orders")]
[Authorize(Roles = "Admin")]
public class AdminOrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public AdminOrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // GET: api/admin/orders
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();

        return Ok(new
        {
            Success = true,
            Data = orders
        });
    }


    // GET: api/admin/orders/5
    [HttpGet("{orderId:int}")]
    public async Task<IActionResult> GetOrder(int orderId)
    {
        var result = await _orderService
            .GetOrderByIdForAdminAsync(orderId);

        return Ok(new
        {
            Success = true,
            Data = result
        });
    }

    // PUT: api/admin/orders/5/status
    [HttpPut("{orderId:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int orderId,
        [FromBody] OrderStatus status)
    {
        var result = await _orderService
            .UpdateOrderStatusAsync(orderId, status);

        return Ok(new
        {
            Success = true,
            Message = "Order status updated successfully.",
            Data = result
        });
    }
}