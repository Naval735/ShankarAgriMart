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
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    // GET: api/Cart
    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = GetUserId();

        var cart = await _cartService.GetCartAsync(userId);

        return Ok(cart);
    }

    // POST: api/Cart/items
    [HttpPost("items")]
    public async Task<IActionResult> AddItem(
        [FromBody] AddCartItemRequest request)
    {
        var userId = GetUserId();

        var cart = await _cartService.AddItemAsync(
            userId,
            request);

        return Ok(cart);
    }

    // PUT: api/Cart/items/1
    [HttpPut("items/{cartItemId:int}")]
    public async Task<IActionResult> UpdateItem(
        int cartItemId,
        [FromBody] int quantity)
    {
        var userId = GetUserId();

        var cart = await _cartService.UpdateItemAsync(
            userId,
            cartItemId,
            quantity);

        return Ok(cart);
    }

    // DELETE: api/Cart/items/1
    [HttpDelete("items/{cartItemId:int}")]
    public async Task<IActionResult> RemoveItem(int cartItemId)
    {
        var userId = GetUserId();

        await _cartService.RemoveItemAsync(
            userId,
            cartItemId);

        return NoContent();
    }

    // DELETE: api/Cart
    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        var userId = GetUserId();

        await _cartService.ClearCartAsync(userId);

        return NoContent();
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