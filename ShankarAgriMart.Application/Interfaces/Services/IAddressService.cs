using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;

namespace ShankarAgriMart.Application.Interfaces.Services;

public interface IAddressService
{
    Task<List<AddressResponse>> GetMyAddressesAsync(int userId);

    Task<AddressResponse> GetByIdAsync(
        int userId,
        int addressId);

    Task<AddressResponse> CreateAsync(
        int userId,
        CreateAddressRequest request);

    Task UpdateAsync(
        int userId,
        int addressId,
        UpdateAddressRequest request);

    Task DeleteAsync(
        int userId,
        int addressId);
}