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

namespace ShankarAgriMart.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserProfileResponse> GetProfileAsync(
        int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            throw new NotFoundException("User not found.");

        return Map(user);
    }

    public async Task<UserProfileResponse> UpdateProfileAsync(
        int userId,
        UpdateUserProfileRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            throw new NotFoundException("User not found.");

        var phone = request.Phone.Trim();

        // Don't allow the user's phone number to be used
        // by another account.
        if (!string.Equals(
                user.Phone,
                phone,
                StringComparison.OrdinalIgnoreCase))
        {
            var phoneExists =
                await _userRepository.PhoneExistsAsync(phone);

            if (phoneExists)
                throw new ArgumentException(
                    "Phone number is already registered.");
        }

        user.FirstName = request.FirstName.Trim();

        user.LastName = string.IsNullOrWhiteSpace(request.LastName)
            ? null
            : request.LastName.Trim();

        user.Phone = phone;

        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        return Map(user);
    }

    private static UserProfileResponse Map(
        Domain.Entities.User user)
    {
        return new UserProfileResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            EmailVerified = user.EmailVerified,
            Role = user.Role?.RoleName ?? string.Empty
        };
    }
}