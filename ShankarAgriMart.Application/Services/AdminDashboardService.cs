using ShankarAgriMart.Application.DTOs.Response;
using ShankarAgriMart.Application.Interfaces.Repositories;
using ShankarAgriMart.Application.Interfaces.Services;
using ShankarAgriMart.Domain.Enums;

namespace ShankarAgriMart.Application.Services;

public class AdminDashboardService : IAdminDashboardService
{
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;

    public AdminDashboardService(
        IProductRepository productRepository,
        IUserRepository userRepository,
        IOrderRepository orderRepository)
    {
        _productRepository = productRepository;
        _userRepository = userRepository;
        _orderRepository = orderRepository;
    }

    public async Task<AdminDashboardResponse> GetDashboardAsync()
    {
        var products = await _productRepository.GetAllAsync();
        var users = await _userRepository.GetAllAsync();
        var orders = await _orderRepository.GetAllAsync();

        return new AdminDashboardResponse
        {
            TotalProducts = products.Count,

            TotalCustomers = users.Count(x =>
                x.Role != null &&
                x.Role.RoleName == "Customer"),

            TotalOrders = orders.Count,

            TotalRevenue = orders
                .Where(x => x.OrderStatus != OrderStatus.Cancelled)
                .Sum(x => x.GrandTotal),

            PendingOrders = orders.Count(x =>
                x.OrderStatus == OrderStatus.Placed),

            ConfirmedOrders = orders.Count(x =>
                x.OrderStatus == OrderStatus.Confirmed),

            ShippedOrders = orders.Count(x =>
                x.OrderStatus == OrderStatus.Shipped),

            DeliveredOrders = orders.Count(x =>
                x.OrderStatus == OrderStatus.Delivered),

            CancelledOrders = orders.Count(x =>
                x.OrderStatus == OrderStatus.Cancelled)
        };
    }
}