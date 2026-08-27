using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;
using ShankarAgriMart.Domain.Enums;

namespace ShankarAgriMart.Application.Interfaces.Services;

public interface IOrderService
{
    Task<OrderResponse> CreateOrderAsync(
        int userId,
        CreateOrderRequest request);

    Task<OrderResponse> GetOrderByIdAsync(
        int userId,
        int orderId);

    Task<List<OrderResponse>> GetMyOrdersAsync(
        int userId);

    Task<OrderResponse> CancelOrderAsync(
        int userId,
        int orderId);

    Task<List<OrderResponse>> GetAllOrdersAsync();

    Task<OrderResponse> UpdateOrderStatusAsync(
        int orderId,
        OrderStatus status);
}