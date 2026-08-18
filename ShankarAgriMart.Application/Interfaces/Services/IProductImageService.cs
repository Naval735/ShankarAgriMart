using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;

namespace ShankarAgriMart.Application.Interfaces.Services;

public interface IProductImageService
{
    Task<List<ProductImageResponse>> GetByProductIdAsync(int productId);

    Task<ProductImageResponse> AddAsync(
        int productId,
        AddProductImageRequest request);

    Task<ProductImageResponse> UpdateAsync(
        int productId,
        int imageId,
        AddProductImageRequest request);

    Task DeleteAsync(
        int productId,
        int imageId);
}
