using ShankarAgriMart.Domain.Common;

namespace ShankarAgriMart.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;

    public string? LastName { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public int RoleId { get; set; }

    public bool IsActive { get; set; } = true;

    public bool EmailVerified { get; set; }

    public Role Role { get; set; } = null!;

    public ICollection<Address> Addresses { get; set; }
        = new List<Address>();
}