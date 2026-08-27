using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;
using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();

    Task<Product?> GetByIdAsync(int id);

    Task<Product?> GetBySkuAsync(string sku);

    Task<Product?> GetBySlugAsync(string slug);

    Task<Product> AddAsync(Product product);

    Task UpdateAsync(Product product);

    Task DeleteAsync(Product product);

    Task<PagedResponse<Product>> GetFilteredAsync(
        ProductFilterRequest request);
}