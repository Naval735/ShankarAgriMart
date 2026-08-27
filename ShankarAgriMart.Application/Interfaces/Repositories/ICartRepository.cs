using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Application.Interfaces.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(int userId);

    Task<Cart?> GetByIdAsync(int id);

    Task<Cart> CreateAsync(Cart cart);

    Task<CartItem?> GetItemAsync(
        int cartId,
        int productId);

    Task<CartItem?> GetItemByIdAsync(int cartItemId);

    Task<CartItem> AddItemAsync(CartItem item);

    Task UpdateItemAsync(CartItem item);

    Task RemoveItemAsync(CartItem item);
    Task RestoreItemAsync(CartItem item);
}