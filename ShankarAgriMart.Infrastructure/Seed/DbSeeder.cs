using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using ShankarAgriMart.Domain.Entities;
using ShankarAgriMart.Infrastructure.Data;

namespace ShankarAgriMart.Infrastructure.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Make sure database is available
        await context.Database.MigrateAsync();

        // Get roles
        var adminRole = await context.Roles
            .FirstOrDefaultAsync(r => r.RoleName == "Admin");

        var customerRole = await context.Roles
            .FirstOrDefaultAsync(r => r.RoleName == "Customer");

        if (adminRole == null || customerRole == null)
        {
            throw new InvalidOperationException(
                "Required roles were not found.");
        }

        // Check whether admin already exists
        var adminExists = await context.Users
            .AnyAsync(u =>
                u.Email == "admin@shankaragrimart.com");

        if (!adminExists)
        {
            var admin = new User
            {
                FirstName = "System",
                LastName = "Admin",
                Email = "admin@shankaragrimart.com",
                Phone = "9999999999",

                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword("Admin@12345"),

                RoleId = adminRole.Id,

                IsActive = true,
                EmailVerified = true
            };

            await context.Users.AddAsync(admin);

            await context.SaveChangesAsync();
        }
    }
}