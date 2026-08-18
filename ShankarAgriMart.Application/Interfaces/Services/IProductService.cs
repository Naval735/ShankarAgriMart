using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;

namespace ShankarAgriMart.Application.Interfaces.Services;

public interface IProductService
{
    Task<List<ProductResponse>> GetAllAsync();

    Task<ProductResponse> GetByIdAsync(int id);

    Task<ProductResponse> CreateAsync(CreateProductRequest request);

    Task<ProductResponse> UpdateAsync(
        int id,
        UpdateProductRequest request);

    Task DeleteAsync(int id);
}