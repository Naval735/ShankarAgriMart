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

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
    }

    public async Task<List<ProductResponse>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();

        return products.Select(Map).ToList();
    }

    public async Task<ProductResponse> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            throw new NotFoundException("Product not found.");

        return Map(product);
    }

    public async Task<ProductResponse> CreateAsync(
        CreateProductRequest request)
    {
        var category = await _categoryRepository
            .GetByIdAsync(request.CategoryId);

        if (category == null)
            throw new NotFoundException("Category not found.");

        var brand = await _brandRepository
            .GetByIdAsync(request.BrandId);

        if (brand == null)
            throw new NotFoundException("Brand not found.");

        var slug = GenerateSlug(request.Name);

        var existingSlug = await _productRepository
            .GetBySlugAsync(slug);

        if (existingSlug != null)
            throw new ConflictException(
                "A product with this name already exists.");

        var sku = await GenerateSkuAsync();

        var product = new Product
        {
            CategoryId = request.CategoryId,
            BrandId = request.BrandId,

            Name = request.Name.Trim(),
            Slug = slug,
            SKU = sku,

            ShortDescription = request.ShortDescription?.Trim(),
            Description = request.Description?.Trim(),

            MRP = request.MRP,
            SellingPrice = request.SellingPrice,
            GST = request.GST,

            Stock = request.Stock,

            Weight = request.Weight,
            Unit = request.Unit?.Trim(),

            ActiveIngredient = request.ActiveIngredient?.Trim(),
            Dosage = request.Dosage?.Trim(),
            ApplicationMethod = request.ApplicationMethod?.Trim(),
            Benefits = request.Benefits?.Trim(),
            UsageInstructions = request.UsageInstructions?.Trim(),
            SafetyPrecautions = request.SafetyPrecautions?.Trim(),

            Manufacturer = request.Manufacturer?.Trim(),
            CountryOfOrigin = request.CountryOfOrigin?.Trim(),

            ExpiryDate = request.ExpiryDate,

            IsFeatured = request.IsFeatured,
            IsActive = true
        };

        var created = await _productRepository.AddAsync(product);

        // Reload so navigation properties are available.
        var result = await _productRepository.GetByIdAsync(created.Id);

        if (result == null)
            throw new NotFoundException(
                "Product could not be retrieved after creation.");

        return Map(result);
    }

    public async Task<ProductResponse> UpdateAsync(
        int id,
        UpdateProductRequest request)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            throw new NotFoundException("Product not found.");

        var category = await _categoryRepository
            .GetByIdAsync(request.CategoryId);

        if (category == null)
            throw new NotFoundException("Category not found.");

        var brand = await _brandRepository
            .GetByIdAsync(request.BrandId);

        if (brand == null)
            throw new NotFoundException("Brand not found.");

        var newSlug = GenerateSlug(request.Name);

        if (!newSlug.Equals(
                product.Slug,
                StringComparison.OrdinalIgnoreCase))
        {
            var existingSlug = await _productRepository
                .GetBySlugAsync(newSlug);

            if (existingSlug != null &&
                existingSlug.Id != id)
            {
                throw new ConflictException(
                    "Another product with this name already exists.");
            }

            product.Slug = newSlug;
        }

        product.CategoryId = request.CategoryId;
        product.BrandId = request.BrandId;

        product.Name = request.Name.Trim();

        product.ShortDescription =
            request.ShortDescription?.Trim();

        product.Description =
            request.Description?.Trim();

        product.MRP = request.MRP;
        product.SellingPrice = request.SellingPrice;
        product.GST = request.GST;

        product.Stock = request.Stock;

        product.Weight = request.Weight;
        product.Unit = request.Unit?.Trim();

        product.ActiveIngredient =
            request.ActiveIngredient?.Trim();

        product.Dosage =
            request.Dosage?.Trim();

        product.ApplicationMethod =
            request.ApplicationMethod?.Trim();

        product.Benefits =
            request.Benefits?.Trim();

        product.UsageInstructions =
            request.UsageInstructions?.Trim();

        product.SafetyPrecautions =
            request.SafetyPrecautions?.Trim();

        product.Manufacturer =
            request.Manufacturer?.Trim();

        product.CountryOfOrigin =
            request.CountryOfOrigin?.Trim();

        product.ExpiryDate = request.ExpiryDate;

        product.IsFeatured = request.IsFeatured;
        product.IsActive = request.IsActive;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product);

        var result = await _productRepository.GetByIdAsync(id);

        if (result == null)
            throw new NotFoundException("Product not found.");

        return Map(result);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            throw new NotFoundException("Product not found.");

        await _productRepository.DeleteAsync(product);
    }

    private async Task<string> GenerateSkuAsync()
    {
        var number = 1;

        while (true)
        {
            var sku = $"AGR-{number:D5}";

            var existing = await _productRepository
                .GetBySkuAsync(sku);

            if (existing == null)
                return sku;

            number++;
        }
    }

    private static string GenerateSlug(string name)
    {
        var slug = name.Trim().ToLowerInvariant();

        slug = string.Join(
            "-",
            slug.Split(
                new[] { ' ', '-', '_' },
                StringSplitOptions.RemoveEmptyEntries));

        return slug;
    }

    private static ProductResponse Map(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,

            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty,

            BrandId = product.BrandId,
            BrandName = product.Brand?.Name ?? string.Empty,

            Name = product.Name,
            Slug = product.Slug,
            SKU = product.SKU,

            ShortDescription = product.ShortDescription,
            Description = product.Description,

            MRP = product.MRP,
            SellingPrice = product.SellingPrice,
            GST = product.GST,

            Stock = product.Stock,

            Weight = product.Weight,
            Unit = product.Unit,

            ActiveIngredient = product.ActiveIngredient,
            Dosage = product.Dosage,
            ApplicationMethod = product.ApplicationMethod,
            Benefits = product.Benefits,
            UsageInstructions = product.UsageInstructions,
            SafetyPrecautions = product.SafetyPrecautions,

            Manufacturer = product.Manufacturer,
            CountryOfOrigin = product.CountryOfOrigin,

            ExpiryDate = product.ExpiryDate,

            IsFeatured = product.IsFeatured,
            IsActive = product.IsActive,

            Images = product.ProductImages
                .OrderBy(x => x.DisplayOrder)
                .Select(x => x.ImageUrl)
                .ToList()
        };
    }
}