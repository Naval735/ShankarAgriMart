using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.Interfaces.Services;

namespace ShankarAgriMart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Customer")]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    // GET: api/Address
    [HttpGet]
    public async Task<IActionResult> GetMyAddresses()
    {
        var userId = GetUserId();

        var addresses = await _addressService
            .GetMyAddressesAsync(userId);

        return Ok(addresses);
    }

    // GET: api/Address/1
    [HttpGet("{addressId:int}")]
    public async Task<IActionResult> GetAddress(int addressId)
    {
        var userId = GetUserId();

        var address = await _addressService
            .GetByIdAsync(userId, addressId);

        return Ok(address);
    }

    // POST: api/Address
    [HttpPost]
    public async Task<IActionResult> CreateAddress(
        [FromBody] CreateAddressRequest request)
    {
        var userId = GetUserId();

        var address = await _addressService
            .CreateAsync(userId, request);

        return Ok(address);
    }

    // PUT: api/Address/1
    [HttpPut("{addressId:int}")]
    public async Task<IActionResult> UpdateAddress(
        int addressId,
        [FromBody] UpdateAddressRequest request)
    {
        var userId = GetUserId();

        await _addressService.UpdateAsync(
            userId,
            addressId,
            request);

        return NoContent();
    }

    // DELETE: api/Address/1
    [HttpDelete("{addressId:int}")]
    public async Task<IActionResult> DeleteAddress(
        int addressId)
    {
        var userId = GetUserId();

        await _addressService.DeleteAsync(
            userId,
            addressId);

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