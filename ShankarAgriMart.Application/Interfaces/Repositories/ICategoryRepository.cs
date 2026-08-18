using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Application.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();

    Task<Category?> GetByIdAsync(int id);

    Task<Category?> GetByNameAsync(string name);

    Task<Category> AddAsync(Category category);

    Task UpdateAsync(Category category);

    Task DeleteAsync(Category category);
}