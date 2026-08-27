using ShankarAgriMart.Application.Common.Exceptions;
using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;
using ShankarAgriMart.Application.Interfaces.Repositories;
using ShankarAgriMart.Application.Interfaces.Services;
using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartService(
        ICartRepository cartRepository,
        IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<CartResponse> GetCartAsync(int userId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);

        if (cart == null)
        {
            return new CartResponse
            {
                UserId = userId,
                Items = new List<CartItemResponse>(),
                TotalAmount = 0
            };
        }

        return Map(cart);
    }

    public async Task<CartResponse> AddItemAsync(
        int userId,
        AddCartItemRequest request)
    {
        if (request.Quantity <= 0)
            throw new ArgumentException(
                "Quantity must be greater than zero.");

        var product = await _productRepository
            .GetByIdAsync(request.ProductId);

        if (product == null || !product.IsActive)
            throw new NotFoundException(
                "Product not found or inactive.");

        if (product.Stock < request.Quantity)
            throw new ArgumentException(
                "Insufficient stock.");

        var cart = await _cartRepository
            .GetByUserIdAsync(userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                IsDeleted = false
            };

            cart = await _cartRepository.CreateAsync(cart);
        }

        var existingItem = await _cartRepository.GetItemAsync(
            cart.Id,
            request.ProductId);

        if (existingItem != null)
        {
            var newQuantity =
                existingItem.Quantity + request.Quantity;

            if (newQuantity > product.Stock)
                throw new ArgumentException(
                    "Requested quantity exceeds available stock.");

            existingItem.Quantity = newQuantity;
            existingItem.UnitPrice = product.SellingPrice;
            existingItem.UpdatedAt = DateTime.UtcNow;

            // Restore previously deleted item
            if (existingItem.IsDeleted)
            {
                await _cartRepository.RestoreItemAsync(existingItem);
            }
            else
            {
                await _cartRepository.UpdateItemAsync(existingItem);
            }
        }
        else
        {
            var item = new CartItem
            {
                CartId = cart.Id,
                ProductId = product.Id,
                Quantity = request.Quantity,
                UnitPrice = product.SellingPrice
            };

            await _cartRepository.AddItemAsync(item);
        }

        var updatedCart = await _cartRepository
            .GetByUserIdAsync(userId);

        return Map(updatedCart!);
    }

    public async Task<CartResponse> UpdateItemAsync(
        int userId,
        int cartItemId,
        int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException(
                "Quantity must be greater than zero.");

        var cart = await _cartRepository
            .GetByUserIdAsync(userId);

        if (cart == null)
            throw new NotFoundException("Cart not found.");

        var item = await _cartRepository
            .GetItemByIdAsync(cartItemId);

        if (item == null || item.CartId != cart.Id)
            throw new NotFoundException(
                "Cart item not found.");

        var product = await _productRepository
            .GetByIdAsync(item.ProductId);

        if (product == null || !product.IsActive)
            throw new NotFoundException(
                "Product not found or inactive.");

        if (quantity > product.Stock)
            throw new ArgumentException(
                "Requested quantity exceeds available stock.");

        item.Quantity = quantity;
        item.UnitPrice = product.SellingPrice;
        item.UpdatedAt = DateTime.UtcNow;

        await _cartRepository.UpdateItemAsync(item);

        var updatedCart = await _cartRepository
            .GetByUserIdAsync(userId);

        return Map(updatedCart!);
    }

    public async Task RemoveItemAsync(
        int userId,
        int cartItemId)
    {
        var cart = await _cartRepository
            .GetByUserIdAsync(userId);

        if (cart == null)
            throw new NotFoundException("Cart not found.");

        var item = await _cartRepository
            .GetItemByIdAsync(cartItemId);

        if (item == null || item.CartId != cart.Id)
            throw new NotFoundException(
                "Cart item not found.");

        await _cartRepository.RemoveItemAsync(item);
    }

    public async Task ClearCartAsync(int userId)
    {
        var cart = await _cartRepository
            .GetByUserIdAsync(userId);

        if (cart == null)
            return;

        foreach (var item in cart.CartItems)
        {
            if (!item.IsDeleted)
            {
                await _cartRepository.RemoveItemAsync(item);
            }
        }
    }

    private static CartResponse Map(Cart cart)
    {
        var items = cart.CartItems
            .Where(x => !x.IsDeleted)
            .Select(item => new CartItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,

                ProductName =
                    item.Product?.Name ?? string.Empty,

                ProductImage =
                    item.Product?.ProductImages
                        .OrderByDescending(x => x.IsPrimary)
                        .ThenBy(x => x.DisplayOrder)
                        .Select(x => x.ImageUrl)
                        .FirstOrDefault(),

                Quantity = item.Quantity,

                UnitPrice = item.UnitPrice,

                TotalPrice =
                    item.UnitPrice * item.Quantity
            })
            .ToList();

        return new CartResponse
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = items,
            TotalAmount = items.Sum(x => x.TotalPrice)
        };
    }
}