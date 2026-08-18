using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
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
}