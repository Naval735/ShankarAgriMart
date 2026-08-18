using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Application.Interfaces.Repositories;

public interface IBrandRepository
{
    Task<List<Brand>> GetAllAsync();

    Task<Brand?> GetByIdAsync(int id);

    Task<Brand?> GetByNameAsync(string name);

    Task<Brand> AddAsync(Brand brand);

    Task UpdateAsync(Brand brand);

    Task DeleteAsync(Brand brand);
}