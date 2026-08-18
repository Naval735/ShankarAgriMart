using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;

namespace ShankarAgriMart.Application.Interfaces.Services;

public interface IBrandService
{
    Task<List<BrandResponse>> GetAllAsync();

    Task<BrandResponse> GetByIdAsync(int id);

    Task<BrandResponse> CreateAsync(CreateBrandRequest request);

    Task<BrandResponse> UpdateAsync(
        int id,
        UpdateBrandRequest request);

    Task DeleteAsync(int id);
}