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

public class InventoryTransactionRepository : IInventoryTransactionRepository
{
    private readonly AppDbContext _context;

    public InventoryTransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<InventoryTransaction>> GetByProductIdAsync(
        int productId)
    {
        return await _context.InventoryTransactions
            .AsNoTracking()
            .Include(x => x.Product)
            .Where(x =>
                x.ProductId == productId &&
                !x.IsDeleted)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<InventoryTransaction?> GetByIdAsync(int id)
    {
        return await _context.InventoryTransactions
            .AsNoTracking()
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                !x.IsDeleted);
    }

    public async Task<InventoryTransaction> AddAsync(
        InventoryTransaction transaction)
    {
        await _context.InventoryTransactions.AddAsync(transaction);
        await _context.SaveChangesAsync();

        return transaction;
    }
}