using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Application.Interfaces.Repositories;

public interface IProductImageRepository
{
    Task<List<ProductImage>> GetByProductIdAsync(int productId);

    Task<ProductImage?> GetByIdAsync(int id);

    Task<ProductImage> AddAsync(ProductImage image);

    Task UpdateAsync(ProductImage image);

    Task DeleteAsync(ProductImage image);
}