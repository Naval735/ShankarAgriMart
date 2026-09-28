
using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;

namespace ShankarAgriMart.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetAllAsync();

    Task<CategoryResponse> GetByIdAsync(int id);

    Task<CategoryResponse> CreateAsync(CreateCategoryRequest request);

    Task<CategoryResponse> UpdateAsync(
        int id,
        UpdateCategoryRequest request);

    Task DeleteAsync(int id);
}