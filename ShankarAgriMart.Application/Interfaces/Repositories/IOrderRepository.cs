using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Application.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int orderId);

    Task<Order?> GetByIdForUserAsync(
        int orderId,
        int userId);

    Task<List<Order>> GetByUserIdAsync(
        int userId);

    Task<Order> CreateAsync(Order order);

    Task UpdateAsync(Order order);

    Task<List<Order>> GetAllAsync();
}