using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Application.Interfaces.Repositories;

public interface IAddressRepository
{
    Task<Address?> GetByIdAsync(int addressId);

    Task<Address?> GetByIdForUserAsync(
        int addressId,
        int userId);

    Task<List<Address>> GetByUserIdAsync(
        int userId);

    Task<Address> CreateAsync(Address address);

    Task UpdateAsync(Address address);

    Task DeleteAsync(Address address);
}