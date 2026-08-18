using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShankarAgriMart.Application.Common;
using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.Interfaces.Services;

namespace ShankarAgriMart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    // Public - customers can view categories
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();

        return Ok(new ApiResponse<object>(
            true,
            "Categories retrieved successfully.",
            result));
    }

    // Public
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        return Ok(new ApiResponse<object>(
            true,
            "Category retrieved successfully.",
            result));
    }

    // Admin only
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCategoryRequest request)
    {
        var result = await _service.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            new ApiResponse<object>(
                true,
                "Category created successfully.",
                result));
    }

    // Admin only
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateCategoryRequest request)
    {
        var result = await _service.UpdateAsync(id, request);

        return Ok(new ApiResponse<object>(
            true,
            "Category updated successfully.",
            result));
    }

    // Admin only
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);

        return Ok(new ApiResponse<object>(
            true,
            "Category deleted successfully."));
    }
}