using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Application.Common.Exceptions;
using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;
using ShankarAgriMart.Application.Interfaces.Repositories;
using ShankarAgriMart.Application.Interfaces.Services;
using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CategoryResponse>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();

        return categories.Select(Map).ToList();
    }

    public async Task<CategoryResponse> GetByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);

        if (category == null)
            throw new NotFoundException(
                "Category not found.");

        return Map(category);
    }

    public async Task<CategoryResponse> CreateAsync(
        CreateCategoryRequest request)
    {
        var existing = await _repository
            .GetByNameAsync(request.Name);

        if (existing != null)
            throw new ConflictException(
                "Category already exists.");

        var category = new Category
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            ImageUrl = request.ImageUrl?.Trim(),
            DisplayOrder = request.DisplayOrder,
            IsActive = true
        };

        var created = await _repository.AddAsync(category);

        return Map(created);
    }

    public async Task<CategoryResponse> UpdateAsync(
        int id,
        UpdateCategoryRequest request)
    {
        var category = await _repository.GetByIdAsync(id);

        if (category == null)
            throw new NotFoundException(
                "Category not found.");

        var duplicate = await _repository
            .GetByNameAsync(request.Name);

        if (duplicate != null && duplicate.Id != id)
            throw new ConflictException(
                "Another category with this name already exists.");

        category.Name = request.Name.Trim();
        category.Description = request.Description?.Trim();
        category.ImageUrl = request.ImageUrl?.Trim();
        category.DisplayOrder = request.DisplayOrder;
        category.IsActive = request.IsActive;

        await _repository.UpdateAsync(category);

        return Map(category);
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);

        if (category == null)
            throw new NotFoundException(
                "Category not found.");

        await _repository.DeleteAsync(category);
    }

    private static CategoryResponse Map(Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            DisplayOrder = category.DisplayOrder,
            IsActive = category.IsActive
        };
    }
}