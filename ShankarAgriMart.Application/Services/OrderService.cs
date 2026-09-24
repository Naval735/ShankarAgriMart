using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Application.Common.Exceptions;
using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;
using ShankarAgriMart.Application.Interfaces.Repositories;
using ShankarAgriMart.Application.Interfaces.Services;
using ShankarAgriMart.Domain.Entities;
using ShankarAgriMart.Domain.Enums;

namespace ShankarAgriMart.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IProductRepository _productRepository;
    private readonly IInventoryTransactionRepository _transactionRepository;

    public OrderService(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IAddressRepository addressRepository,
        IProductRepository productRepository,
        IInventoryTransactionRepository transactionRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _addressRepository = addressRepository;
        _productRepository = productRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<OrderResponse> CreateOrderAsync(
        int userId,
        CreateOrderRequest request)
    {
        // 1. Validate payment method
        if (request.PaymentMethod != "ONLINE" &&
            request.PaymentMethod != "COD")
        {
            throw new ArgumentException(
                "Invalid payment method.");
        }

        // 2. Validate customer's address
        var address = await _addressRepository.GetByIdForUserAsync(
            request.AddressId,
            userId);

        if (address == null)
            throw new NotFoundException(
                "Address not found.");

        // 3. Get customer's cart
        var cart = await _cartRepository.GetByUserIdAsync(userId);

        if (cart == null || !cart.CartItems.Any(x => !x.IsDeleted))
            throw new ArgumentException(
                "Cart is empty.");

        var cartItems = cart.CartItems
            .Where(x => !x.IsDeleted)
            .ToList();

        // 4. Validate stock and calculate subtotal
        decimal subTotal = 0;

        foreach (var cartItem in cartItems)
        {
            var product = await _productRepository
                .GetByIdAsync(cartItem.ProductId);

            if (product == null || !product.IsActive)
                throw new NotFoundException(
                    $"Product '{cartItem.ProductId}' not found or inactive.");

            if (cartItem.Quantity > product.Stock)
                throw new ArgumentException(
                    $"Insufficient stock for product '{product.Name}'.");

            // Always use current database price
            cartItem.UnitPrice = product.SellingPrice;

            subTotal +=
                product.SellingPrice * cartItem.Quantity;
        }

        // 5. Calculate GST
        decimal gst = 0;

        foreach (var cartItem in cartItems)
        {
            var product = await _productRepository
                .GetByIdAsync(cartItem.ProductId);

            if (product != null)
            {
                gst +=
                    (cartItem.UnitPrice *
                     cartItem.Quantity *
                     product.GST) / 100;
            }
        }

        // 6. Delivery charge
        decimal deliveryCharge =
            subTotal >= 1000 ? 0 : 50;

        // 7. Discount
        decimal discount = 0;

        // 8. Grand total
        decimal grandTotal =
            subTotal +
            gst +
            deliveryCharge -
            discount;

        // 9. Create order
        var order = new Order
        {
            OrderNumber = GenerateOrderNumber(),
            UserId = userId,
            AddressId = request.AddressId,

            SubTotal = subTotal,
            GST = gst,
            DeliveryCharge = deliveryCharge,
            Discount = discount,
            GrandTotal = grandTotal,

            PaymentStatus = PaymentStatus.Pending,
            OrderStatus = OrderStatus.Placed,

            PaymentMethod = request.PaymentMethod == "COD"
          ? PaymentMethod.CashOnDelivery
          : PaymentMethod.Razorpay,

            OrderDate = DateTime.UtcNow,

            OrderItems = new List<OrderItem>()
        };

        // 10. Create order items
        foreach (var cartItem in cartItems)
        {
            var product = await _productRepository
                .GetByIdAsync(cartItem.ProductId);

            if (product == null)
                continue;

            var itemGST =
                (cartItem.UnitPrice *
                 cartItem.Quantity *
                 product.GST) / 100;

            var itemTotal =
                (cartItem.UnitPrice *
                 cartItem.Quantity) +
                itemGST;

            order.OrderItems.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.UnitPrice,
                GST = itemGST,
                Total = itemTotal
            });

            // Reduce stock
            product.Stock -= cartItem.Quantity;
            product.UpdatedAt = DateTime.UtcNow;

            // Create inventory transaction
            var inventoryTransaction = new InventoryTransaction
            {
                ProductId = product.Id,
                TransactionType = "StockOut",
                Quantity = cartItem.Quantity,
                Remarks =
                    $"Stock deducted for Order {order.OrderNumber}"
            };

            await _transactionRepository
                .AddAsync(inventoryTransaction);
        }

        // 11. Save order
        await _orderRepository.CreateAsync(order);

        // 12. Clear cart
        foreach (var cartItem in cartItems)
        {
            await _cartRepository.RemoveItemAsync(cartItem);
        }

        // 13. Return created order
        return Map(order);
    }

    public async Task<OrderResponse> GetOrderByIdAsync(
        int userId,
        int orderId)
    {
        var order = await _orderRepository
            .GetByIdForUserAsync(orderId, userId);

        if (order == null)
            throw new NotFoundException(
                "Order not found.");

        return Map(order);
    }

    public async Task<OrderResponse> GetOrderByIdForAdminAsync(
        int orderId)
    {
        var order = await _orderRepository
            .GetByIdAsync(orderId);

        if (order == null)
            throw new NotFoundException(
                "Order not found.");

        return Map(order);
    }

    public async Task<List<OrderResponse>> GetMyOrdersAsync(
        int userId)
    {
        var orders = await _orderRepository
            .GetByUserIdAsync(userId);

        return orders
            .Select(Map)
            .ToList();
    }

    public async Task<List<OrderResponse>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();

        return orders
            .Select(Map)
            .ToList();
    }

    public async Task<OrderResponse> UpdateOrderStatusAsync(
        int orderId,
        OrderStatus status)
    {
        var order = await _orderRepository
            .GetByIdAsync(orderId);

        if (order == null)
            throw new NotFoundException(
                "Order not found.");

        if (order.OrderStatus == OrderStatus.Cancelled)
            throw new ArgumentException(
                "Cancelled orders cannot be updated.");

        if (order.OrderStatus == OrderStatus.Delivered)
            throw new ArgumentException(
                "Delivered orders cannot be updated.");

        // Prevent moving backwards
        if (status != OrderStatus.Cancelled &&
            status < order.OrderStatus)
        {
            throw new ArgumentException(
                $"Order cannot move from {order.OrderStatus} to {status}.");
        }

        // Admin cancellation
        if (status == OrderStatus.Cancelled)
        {
            foreach (var orderItem in order.OrderItems
                .Where(x => !x.IsDeleted))
            {
                var product = await _productRepository
                    .GetByIdAsync(orderItem.ProductId);

                if (product == null)
                    throw new NotFoundException(
                        $"Product '{orderItem.ProductId}' not found.");

                product.Stock += orderItem.Quantity;
                product.UpdatedAt = DateTime.UtcNow;

                var inventoryTransaction =
                    new InventoryTransaction
                    {
                        ProductId = product.Id,
                        TransactionType = "StockIn",
                        Quantity = orderItem.Quantity,
                        Remarks =
                            $"Stock restored for cancelled Order {order.OrderNumber}"
                    };

                await _transactionRepository
                    .AddAsync(inventoryTransaction);
            }
        }

        order.OrderStatus = status;
        order.UpdatedAt = DateTime.UtcNow;

        await _orderRepository.UpdateAsync(order);

        return Map(order);
    }

    // =====================================================
    // Cancel Order
    // =====================================================

    public async Task<OrderResponse> CancelOrderAsync(
        int userId,
        int orderId)
    {
        // 1. Get the user's order
        var order = await _orderRepository
            .GetByIdForUserAsync(orderId, userId);

        if (order == null)
            throw new NotFoundException(
                "Order not found.");

        // 2. Check if already cancelled
        if (order.OrderStatus == OrderStatus.Cancelled)
            throw new ArgumentException(
                "Order is already cancelled.");

        // 3. Do not allow cancellation after shipping
        if (order.OrderStatus == OrderStatus.Shipped ||
            order.OrderStatus == OrderStatus.Delivered)
        {
            throw new ArgumentException(
                "Order cannot be cancelled after it has been shipped.");
        }

        // 4. Restore stock
        foreach (var orderItem in order.OrderItems
            .Where(x => !x.IsDeleted))
        {
            var product = await _productRepository
                .GetByIdAsync(orderItem.ProductId);

            if (product == null)
                throw new NotFoundException(
                    $"Product '{orderItem.ProductId}' not found.");

            product.Stock += orderItem.Quantity;
            product.UpdatedAt = DateTime.UtcNow;

            // 5. Record stock restoration
            var inventoryTransaction = new InventoryTransaction
            {
                ProductId = product.Id,
                TransactionType = "StockIn",
                Quantity = orderItem.Quantity,
                Remarks =
                    $"Stock restored for cancelled Order {order.OrderNumber}"
            };

            await _transactionRepository
                .AddAsync(inventoryTransaction);
        }

        // 6. Update order status
        order.OrderStatus = OrderStatus.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;

        // 7. Save order
        await _orderRepository.UpdateAsync(order);

        // 8. Return updated order
        return Map(order);
    }

    // =====================================================
    // Generate Order Number
    // =====================================================

    private static string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(100, 999)}";
    }

    // =====================================================
    // Map Order → Response
    // =====================================================

    private static OrderResponse Map(Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            UserId = order.UserId,
            AddressId = order.AddressId,

            SubTotal = order.SubTotal,
            GST = order.GST,
            DeliveryCharge = order.DeliveryCharge,
            Discount = order.Discount,
            GrandTotal = order.GrandTotal,

            PaymentStatus = order.PaymentStatus,
            OrderStatus = order.OrderStatus,
            PaymentMethod = order.PaymentMethod,
            OrderDate = order.OrderDate,

            Items = order.OrderItems
                .Where(x => !x.IsDeleted)
                .Select(x => new OrderItemResponse
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product?.Name ?? string.Empty,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    GST = x.GST,
                    Total = x.Total
                })
                .ToList()
        };
    }
}