using ShankarAgriMart.Application.Common.Exceptions;
using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;
using ShankarAgriMart.Application.Interfaces.Repositories;
using ShankarAgriMart.Application.Interfaces.Services;
using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Application.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<List<AddressResponse>> GetMyAddressesAsync(
        int userId)
    {
        var addresses = await _addressRepository
            .GetByUserIdAsync(userId);

        return addresses
            .Select(Map)
            .ToList();
    }

    public async Task<AddressResponse> GetByIdAsync(
        int userId,
        int addressId)
    {
        var address = await _addressRepository
            .GetByIdForUserAsync(addressId, userId);

        if (address == null)
            throw new NotFoundException("Address not found.");

        return Map(address);
    }

    public async Task<AddressResponse> CreateAsync(
        int userId,
        CreateAddressRequest request)
    {
        var address = new Address
        {
            UserId = userId,
            FullName = request.FullName,
            Phone = request.Phone,
            AddressLine1 = request.AddressLine1,
            AddressLine2 = request.AddressLine2,
            City = request.City,
            State = request.State,
            Pincode = request.Pincode,
            IsDefault = request.IsDefault
        };

        var addresses = await _addressRepository
            .GetByUserIdAsync(userId);

        // If this is the first address, make it default.
        if (!addresses.Any())
        {
            address.IsDefault = true;
        }

        // If this address is marked as default,
        // remove default status from existing addresses.
        if (address.IsDefault)
        {
            foreach (var existingAddress in addresses)
            {
                existingAddress.IsDefault = false;
            }
        }

        var createdAddress = await _addressRepository
            .CreateAsync(address);

        return Map(createdAddress);
    }

    public async Task UpdateAsync(
        int userId,
        int addressId,
        UpdateAddressRequest request)
    {
        var address = await _addressRepository
            .GetByIdForUserAsync(addressId, userId);

        if (address == null)
            throw new NotFoundException("Address not found.");

        address.FullName = request.FullName;
        address.Phone = request.Phone;
        address.AddressLine1 = request.AddressLine1;
        address.AddressLine2 = request.AddressLine2;
        address.City = request.City;
        address.State = request.State;
        address.Pincode = request.Pincode;
        address.IsDefault = request.IsDefault;
        address.UpdatedAt = DateTime.UtcNow;

        await _addressRepository.UpdateAsync(address);
    }

    public async Task DeleteAsync(
        int userId,
        int addressId)
    {
        var address = await _addressRepository
            .GetByIdForUserAsync(addressId, userId);

        if (address == null)
            throw new NotFoundException("Address not found.");

        await _addressRepository.DeleteAsync(address);
    }

    private static AddressResponse Map(Address address)
    {
        return new AddressResponse
        {
            Id = address.Id,
            UserId = address.UserId,
            FullName = address.FullName,
            Phone = address.Phone,
            AddressLine1 = address.AddressLine1,
            AddressLine2 = address.AddressLine2,
            City = address.City,
            State = address.State,
            Pincode = address.Pincode,
            IsDefault = address.IsDefault
        };
    }
}