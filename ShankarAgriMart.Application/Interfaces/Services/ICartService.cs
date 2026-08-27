using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;

namespace ShankarAgriMart.Application.Interfaces.Services;

public interface ICartService
{
    Task<CartResponse> GetCartAsync(int userId);

    Task<CartResponse> AddItemAsync(
        int userId,
        AddCartItemRequest request);

    Task<CartResponse> UpdateItemAsync(
        int userId,
        int cartItemId,
        int quantity);

    Task RemoveItemAsync(
        int userId,
        int cartItemId);

    Task ClearCartAsync(int userId);
}