using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShankarAgriMart.Application.Common;
using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.Interfaces.Services;

namespace ShankarAgriMart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    // GET: api/Brand
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var brands = await _brandService.GetAllAsync();

        return Ok(new ApiResponse<object>(
            true,
            "Brands retrieved successfully.",
            brands));
    }

    // GET: api/Brand/1
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var brand = await _brandService.GetByIdAsync(id);

        return Ok(new ApiResponse<object>(
            true,
            "Brand retrieved successfully.",
            brand));
    }

    // POST: api/Brand
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateBrandRequest request)
    {
        var brand = await _brandService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = brand.Id },
            new ApiResponse<object>(
                true,
                "Brand created successfully.",
                brand));
    }

    // PUT: api/Brand/1
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateBrandRequest request)
    {
        var brand = await _brandService.UpdateAsync(id, request);

        return Ok(new ApiResponse<object>(
            true,
            "Brand updated successfully.",
            brand));
    }

    // DELETE: api/Brand/1
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _brandService.DeleteAsync(id);

        return Ok(new ApiResponse<object>(
            true,
            "Brand deleted successfully."));
    }
}