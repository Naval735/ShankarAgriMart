using Microsoft.EntityFrameworkCore;
using ShankarAgriMart.Application.Interfaces.Repositories;
using ShankarAgriMart.Domain.Entities;
using ShankarAgriMart.Infrastructure.Data;

namespace ShankarAgriMart.Infrastructure.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly AppDbContext _context;

    public BrandRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Brand>> GetAllAsync()
    {
        return await _context.Brands
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<Brand?> GetByIdAsync(int id)
    {
        return await _context.Brands
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                !x.IsDeleted);
    }

    public async Task<Brand?> GetByNameAsync(string name)
    {
        return await _context.Brands
            .FirstOrDefaultAsync(x =>
                x.Name.ToLower() == name.ToLower() &&
                !x.IsDeleted);
    }

    public async Task<Brand> AddAsync(Brand brand)
    {
        await _context.Brands.AddAsync(brand);
        await _context.SaveChangesAsync();

        return brand;
    }

    public async Task UpdateAsync(Brand brand)
    {
        _context.Brands.Update(brand);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Brand brand)
    {
        brand.IsDeleted = true;
        brand.DeletedAt = DateTime.UtcNow;

        _context.Brands.Update(brand);

        await _context.SaveChangesAsync();
    }
}