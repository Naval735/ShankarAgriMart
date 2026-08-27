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

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(int orderId)
    {
        return await _context.Orders
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x =>
                x.Id == orderId &&
                !x.IsDeleted);
    }

    public async Task<Order?> GetByIdForUserAsync(
        int orderId,
        int userId)
    {
        return await _context.Orders
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x =>
                x.Id == orderId &&
                x.UserId == userId &&
                !x.IsDeleted);
    }

    public async Task<List<Order>> GetByUserIdAsync(
        int userId)
    {
        return await _context.Orders
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
            .Include(x => x.Address)
            .Where(x =>
                x.UserId == userId &&
                !x.IsDeleted)
            .OrderByDescending(x => x.OrderDate)
            .ToListAsync();
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
            .Include(x => x.Address)
            .Include(x => x.User)
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.OrderDate)
            .ToListAsync();
    }
    public async Task<Order> CreateAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
}