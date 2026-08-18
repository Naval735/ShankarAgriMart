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

public class ProductImageRepository : IProductImageRepository
{
    private readonly AppDbContext _context;

    public ProductImageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductImage>> GetByProductIdAsync(int productId)
    {
        return await _context.ProductImages
            .AsNoTracking()
            .Where(x => x.ProductId == productId && !x.IsDeleted)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync();
    }

    public async Task<ProductImage?> GetByIdAsync(int id)
    {
        return await _context.ProductImages
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                !x.IsDeleted);
    }

    public async Task<ProductImage> AddAsync(ProductImage image)
    {
        await _context.ProductImages.AddAsync(image);
        await _context.SaveChangesAsync();

        return image;
    }

    public async Task UpdateAsync(ProductImage image)
    {
        _context.ProductImages.Update(image);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProductImage image)
    {
        image.IsDeleted = true;
        image.DeletedAt = DateTime.UtcNow;

        _context.ProductImages.Update(image);
        await _context.SaveChangesAsync();
    }
}