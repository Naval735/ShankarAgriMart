using Microsoft.EntityFrameworkCore;
using ShankarAgriMart.Application.Interfaces.Repositories;
using ShankarAgriMart.Domain.Entities;
using ShankarAgriMart.Infrastructure.Data;

namespace ShankarAgriMart.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly AppDbContext _context;

    public AddressRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Address?> GetByIdAsync(int addressId)
    {
        return await _context.Addresses
            .FirstOrDefaultAsync(x =>
                x.Id == addressId &&
                !x.IsDeleted);
    }

    public async Task<Address?> GetByIdForUserAsync(
        int addressId,
        int userId)
    {
        return await _context.Addresses
            .FirstOrDefaultAsync(x =>
                x.Id == addressId &&
                x.UserId == userId &&
                !x.IsDeleted);
    }

    public async Task<List<Address>> GetByUserIdAsync(
        int userId)
    {
        return await _context.Addresses
            .Where(x =>
                x.UserId == userId &&
                !x.IsDeleted)
            .OrderByDescending(x => x.IsDefault)
            .ThenByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<Address> CreateAsync(Address address)
    {
        await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();

        return address;
    }

    public async Task UpdateAsync(Address address)
    {
        _context.Addresses.Update(address);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Address address)
    {
        address.IsDeleted = true;
        address.DeletedAt = DateTime.UtcNow;

        _context.Addresses.Update(address);
        await _context.SaveChangesAsync();
    }
}