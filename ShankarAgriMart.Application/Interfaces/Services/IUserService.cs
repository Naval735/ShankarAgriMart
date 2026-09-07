using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;

namespace ShankarAgriMart.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserProfileResponse> GetProfileAsync(int userId);

    Task<UserProfileResponse> UpdateProfileAsync(
        int userId,
        UpdateUserProfileRequest request);
}