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

public class BrandService : IBrandService
{
    private readonly IBrandRepository _repository;

    public BrandService(IBrandRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BrandResponse>> GetAllAsync()
    {
        var brands = await _repository.GetAllAsync();

        return brands.Select(Map).ToList();
    }

    public async Task<BrandResponse> GetByIdAsync(int id)
    {
        var brand = await _repository.GetByIdAsync(id);

        if (brand == null)
            throw new NotFoundException("Brand not found.");

        return Map(brand);
    }

    public async Task<BrandResponse> CreateAsync(
        CreateBrandRequest request)
    {
        var existing = await _repository
            .GetByNameAsync(request.Name);

        if (existing != null)
            throw new ConflictException(
                "Brand with this name already exists.");

        var brand = new Brand
        {
            Name = request.Name.Trim(),
            LogoUrl = request.LogoUrl?.Trim(),
            Description = request.Description?.Trim(),
            IsActive = true
        };

        var created = await _repository.AddAsync(brand);

        return Map(created);
    }

    public async Task<BrandResponse> UpdateAsync(
        int id,
        UpdateBrandRequest request)
    {
        var brand = await _repository.GetByIdAsync(id);

        if (brand == null)
            throw new NotFoundException("Brand not found.");

        var existing = await _repository
            .GetByNameAsync(request.Name);

        if (existing != null && existing.Id != id)
            throw new ConflictException(
                "Another brand with this name already exists.");

        brand.Name = request.Name.Trim();
        brand.LogoUrl = request.LogoUrl?.Trim();
        brand.Description = request.Description?.Trim();
        brand.IsActive = request.IsActive;
        brand.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(brand);

        return Map(brand);
    }

    public async Task DeleteAsync(int id)
    {
        var brand = await _repository.GetByIdAsync(id);

        if (brand == null)
            throw new NotFoundException("Brand not found.");

        await _repository.DeleteAsync(brand);
    }

    private static BrandResponse Map(Brand brand)
    {
        return new BrandResponse
        {
            Id = brand.Id,
            Name = brand.Name,
            LogoUrl = brand.LogoUrl,
            Description = brand.Description,
            IsActive = brand.IsActive
        };
    }
}