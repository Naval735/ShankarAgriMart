using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.Interfaces.Services;

namespace ShankarAgriMart.API.Controllers;

[ApiController]
[Route("api/products/{productId:int}/images")]
public class ProductImageController : ControllerBase
{
    private readonly IProductImageService _imageService;

    public ProductImageController(
        IProductImageService imageService)
    {
        _imageService = imageService;
    }

    // GET: api/products/1/images
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetByProductId(int productId)
    {
        var images = await _imageService
            .GetByProductIdAsync(productId);

        return Ok(images);
    }

    // POST: api/products/1/images
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Add(
        int productId,
        [FromBody] AddProductImageRequest request)
    {
        var image = await _imageService
            .AddAsync(productId, request);

        return Ok(image);
    }

    // PUT: api/products/1/images/1
    [HttpPut("{imageId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int productId,
        int imageId,
        [FromBody] AddProductImageRequest request)
    {
        var image = await _imageService
            .UpdateAsync(productId, imageId, request);

        return Ok(image);
    }

    // DELETE: api/products/1/images/1
    [HttpDelete("{imageId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(
        int productId,
        int imageId)
    {
        await _imageService.DeleteAsync(productId, imageId);

        return NoContent();
    }
}