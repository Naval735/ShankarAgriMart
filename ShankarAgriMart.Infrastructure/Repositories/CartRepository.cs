using Microsoft.EntityFrameworkCore;
using ShankarAgriMart.Application.Interfaces.Repositories;
using ShankarAgriMart.Domain.Entities;
using ShankarAgriMart.Infrastructure.Data;

namespace ShankarAgriMart.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByUserIdAsync(int userId)
    {
        return await _context.Carts
            .Include(x => x.CartItems)
                .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                !x.IsDeleted);
    }

    public async Task<Cart?> GetByIdAsync(int id)
    {
        return await _context.Carts
            .Include(x => x.CartItems)
                .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                !x.IsDeleted);
    }

    public async Task<Cart> CreateAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();

        return cart;
    }

    public async Task<CartItem?> GetItemAsync(
        int cartId,
        int productId)
    {
        // Include soft-deleted items also.
        // This allows us to restore an old CartItem
        // instead of inserting a duplicate row.
        return await _context.CartItems
            .FirstOrDefaultAsync(x =>
                x.CartId == cartId &&
                x.ProductId == productId);
    }

    public async Task<CartItem?> GetItemByIdAsync(int cartItemId)
    {
        return await _context.CartItems
            .Include(x => x.Product)
                .ThenInclude(x => x.ProductImages)
            .FirstOrDefaultAsync(x =>
                x.Id == cartItemId &&
                !x.IsDeleted);
    }

    public async Task<CartItem> AddItemAsync(CartItem item)
    {
        await _context.CartItems.AddAsync(item);
        await _context.SaveChangesAsync();

        return item;
    }

    public async Task UpdateItemAsync(CartItem item)
    {
        _context.CartItems.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveItemAsync(CartItem item)
    {
        item.IsDeleted = true;
        item.DeletedAt = DateTime.UtcNow;

        _context.CartItems.Update(item);

        await _context.SaveChangesAsync();
    }

    public async Task RestoreItemAsync(CartItem item)
    {
        item.IsDeleted = false;
        item.DeletedAt = null;
        item.UpdatedAt = DateTime.UtcNow;

        _context.CartItems.Update(item);

        await _context.SaveChangesAsync();
    }
}