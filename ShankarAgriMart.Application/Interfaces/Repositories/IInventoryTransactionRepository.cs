using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Application.Interfaces.Repositories;

public interface IInventoryTransactionRepository
{
    Task<List<InventoryTransaction>> GetByProductIdAsync(int productId);

    Task<InventoryTransaction?> GetByIdAsync(int id);

    Task<InventoryTransaction> AddAsync(
        InventoryTransaction transaction);
}