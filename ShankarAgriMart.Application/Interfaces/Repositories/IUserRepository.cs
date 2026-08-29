using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByIdAsync(int id);

    Task<bool> EmailExistsAsync(string email);

    Task<bool> PhoneExistsAsync(string phone);

    Task<User> AddAsync(User user);

    Task UpdateAsync(User user);
}