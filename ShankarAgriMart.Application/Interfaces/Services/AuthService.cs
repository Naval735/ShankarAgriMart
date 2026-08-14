using BCrypt.Net;
using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;
using ShankarAgriMart.Application.Interfaces.Repositories;
using ShankarAgriMart.Application.Interfaces.Services;
using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ITokenService _tokenService;
    public AuthService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
         ITokenService tokenService
        )
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // 1. Check whether email already exists
        if (await _userRepository.EmailExistsAsync(request.Email))
        {
            throw new Exception("Email is already registered.");
        }

        // 2. Check whether phone already exists
        if (await _userRepository.PhoneExistsAsync(request.Phone))
        {
            throw new Exception("Phone number is already registered.");
        }

        // 3. Find Customer role
        var customerRole =
            await _roleRepository.GetByNameAsync("Customer");

        if (customerRole == null)
        {
            throw new Exception("Customer role was not found.");
        }

        // 4. Hash password
        var passwordHash =
            BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 5. Create user
        var user = new User
        {
            FirstName = request.FirstName.Trim(),

            LastName = string.IsNullOrWhiteSpace(request.LastName)
                ? null
                : request.LastName.Trim(),

            Email = request.Email.Trim().ToLower(),

            Phone = request.Phone.Trim(),

            PasswordHash = passwordHash,

            RoleId = customerRole.Id,

            IsActive = true,

            EmailVerified = false
        };

        // 6. Save user
        var createdUser =
            await _userRepository.AddAsync(user);

        // 7. Return response
        return new AuthResponse
        {
            UserId = createdUser.Id,

            FirstName = createdUser.FirstName,

            Email = createdUser.Email,

            Role = customerRole.RoleName,

            Token = string.Empty,

            ExpiresAt = DateTime.UtcNow
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null || !user.IsActive)
            throw new Exception("Invalid email or password.");

        if (!BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash))
        {
            throw new Exception("Invalid email or password.");
        }

        var (token, expiresAt) =
            _tokenService.GenerateToken(user);

        return new AuthResponse
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            Email = user.Email,
            Role = user.Role.RoleName,
            Token = token,
            ExpiresAt = expiresAt
        };
    }
}