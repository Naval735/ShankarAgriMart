using Microsoft.EntityFrameworkCore;

using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;
using ShankarAgriMart.Application.Interfaces.Repositories;
using ShankarAgriMart.Domain.Entities;
using ShankarAgriMart.Infrastructure.Data;

namespace ShankarAgriMart.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .Include(x => x.ProductImages)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .Include(x => x.ProductImages)
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                !x.IsDeleted);
    }

    public async Task<Product?> GetBySkuAsync(string sku)
    {
        return await _context.Products
            .FirstOrDefaultAsync(x =>
                x.SKU.ToLower() == sku.ToLower() &&
                !x.IsDeleted);
    }

    public async Task<Product?> GetBySlugAsync(string slug)
    {
        return await _context.Products
            .FirstOrDefaultAsync(x =>
                x.Slug.ToLower() == slug.ToLower() &&
                !x.IsDeleted);
    }

    public async Task<Product> AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        product.IsDeleted = true;
        product.DeletedAt = DateTime.UtcNow;

        _context.Products.Update(product);

        await _context.SaveChangesAsync();
    }

    // =====================================================
    // Search + Filter + Sort + Pagination
    // =====================================================

    public async Task<PagedResponse<Product>> GetFilteredAsync(
        ProductFilterRequest request)
    {
        var query = _context.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .Include(x => x.ProductImages)
            .Where(x => !x.IsDeleted && x.IsActive);

        // Search by product name or SKU
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();

            query = query.Where(x =>
                x.Name.ToLower().Contains(search) ||
                x.SKU.ToLower().Contains(search));
        }

        // Category filter
        if (request.CategoryId.HasValue)
        {
            query = query.Where(x =>
                x.CategoryId == request.CategoryId.Value);
        }

        // Brand filter
        if (request.BrandId.HasValue)
        {
            query = query.Where(x =>
                x.BrandId == request.BrandId.Value);
        }

        // Minimum price
        if (request.MinPrice.HasValue)
        {
            query = query.Where(x =>
                x.SellingPrice >= request.MinPrice.Value);
        }

        // Maximum price
        if (request.MaxPrice.HasValue)
        {
            query = query.Where(x =>
                x.SellingPrice <= request.MaxPrice.Value);
        }

        // Featured products
        if (request.IsFeatured.HasValue)
        {
            query = query.Where(x =>
                x.IsFeatured == request.IsFeatured.Value);
        }

        // Sorting
        query = request.SortBy?.Trim().ToLower() switch
        {
            "price" => request.SortDescending
                ? query.OrderByDescending(x => x.SellingPrice)
                : query.OrderBy(x => x.SellingPrice),

            "name" => request.SortDescending
                ? query.OrderByDescending(x => x.Name)
                : query.OrderBy(x => x.Name),

            "newest" => request.SortDescending
                ? query.OrderBy(x => x.CreatedAt)
                : query.OrderByDescending(x => x.CreatedAt),

            "stock" => request.SortDescending
                ? query.OrderByDescending(x => x.Stock)
                : query.OrderBy(x => x.Stock),

            _ => query.OrderBy(x => x.Name)
        };

        // Prevent invalid pagination values
        var page = request.Page < 1
            ? 1
            : request.Page;

        var pageSize = request.PageSize < 1
            ? 12
            : request.PageSize;

        // Prevent unnecessarily large requests
        if (pageSize > 100)
            pageSize = 100;

        var totalItems = await query.CountAsync();

        var totalPages = (int)Math.Ceiling(
            totalItems / (double)pageSize);

        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponse<Product>
        {
            Items = products,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }
}