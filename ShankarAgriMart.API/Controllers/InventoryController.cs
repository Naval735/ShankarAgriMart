using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.Interfaces.Services;

namespace ShankarAgriMart.API.Controllers;

[ApiController]
[Route("api/products/{productId:int}/inventory")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    // GET: api/products/1/inventory
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetHistory(int productId)
    {
        var history = await _inventoryService
            .GetHistoryAsync(productId);

        return Ok(history);
    }

    // POST: api/products/1/inventory
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddTransaction(
        int productId,
        [FromBody] CreateInventoryTransactionRequest request)
    {
        var transaction = await _inventoryService
            .AddTransactionAsync(productId, request);

        return Ok(transaction);
    }
}