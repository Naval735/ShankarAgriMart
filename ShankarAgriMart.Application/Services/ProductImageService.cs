using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Application.Common.Exceptions;
using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;
using ShankarAgriMart.Application.Interfaces.Repositories;
using ShankarAgriMart.Application.Interfaces.Services;
using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Application.Services;

public class ProductImageService : IProductImageService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductImageRepository _imageRepository;

    public ProductImageService(
        IProductRepository productRepository,
        IProductImageRepository imageRepository)
    {
        _productRepository = productRepository;
        _imageRepository = imageRepository;
    }

    public async Task<List<ProductImageResponse>> GetByProductIdAsync(
        int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product == null)
            throw new NotFoundException("Product not found.");

        var images = await _imageRepository
            .GetByProductIdAsync(productId);

        return images.Select(Map).ToList();
    }

    public async Task<ProductImageResponse> AddAsync(
        int productId,
        AddProductImageRequest request)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product == null)
            throw new NotFoundException("Product not found.");

        if (string.IsNullOrWhiteSpace(request.ImageUrl))
            throw new ArgumentException("Image URL is required.");

        var images = await _imageRepository
            .GetByProductIdAsync(productId);

        // If this is the first image, automatically make it primary.
        var isPrimary = request.IsPrimary || images.Count == 0;

        // Only one image can be primary.
        if (isPrimary)
        {
            foreach (var image in images.Where(x => x.IsPrimary))
            {
                image.IsPrimary = false;
                await _imageRepository.UpdateAsync(image);
            }
        }

        var imageEntity = new ProductImage
        {
            ProductId = productId,
            ImageUrl = request.ImageUrl.Trim(),
            IsPrimary = isPrimary,
            DisplayOrder = request.DisplayOrder
        };

        var created = await _imageRepository.AddAsync(imageEntity);

        return Map(created);
    }

    public async Task<ProductImageResponse> UpdateAsync(
        int productId,
        int imageId,
        AddProductImageRequest request)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product == null)
            throw new NotFoundException("Product not found.");

        var image = await _imageRepository.GetByIdAsync(imageId);

        if (image == null || image.ProductId != productId)
            throw new NotFoundException(
                "Product image not found.");

        if (string.IsNullOrWhiteSpace(request.ImageUrl))
            throw new ArgumentException("Image URL is required.");

        var images = await _imageRepository
            .GetByProductIdAsync(productId);

        if (request.IsPrimary)
        {
            foreach (var existingImage in images
                .Where(x => x.Id != imageId && x.IsPrimary))
            {
                existingImage.IsPrimary = false;
                await _imageRepository.UpdateAsync(existingImage);
            }
        }

        image.ImageUrl = request.ImageUrl.Trim();
        image.IsPrimary = request.IsPrimary;
        image.DisplayOrder = request.DisplayOrder;
        image.UpdatedAt = DateTime.UtcNow;

        await _imageRepository.UpdateAsync(image);

        return Map(image);
    }

    public async Task DeleteAsync(
        int productId,
        int imageId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product == null)
            throw new NotFoundException("Product not found.");

        var image = await _imageRepository.GetByIdAsync(imageId);

        if (image == null || image.ProductId != productId)
            throw new NotFoundException(
                "Product image not found.");

        await _imageRepository.DeleteAsync(image);
    }

    private static ProductImageResponse Map(ProductImage image)
    {
        return new ProductImageResponse
        {
            Id = image.Id,
            ProductId = image.ProductId,
            ImageUrl = image.ImageUrl,
            IsPrimary = image.IsPrimary,
            DisplayOrder = image.DisplayOrder
        };
    }
}