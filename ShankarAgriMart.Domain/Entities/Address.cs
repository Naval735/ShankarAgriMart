using ShankarAgriMart.Domain.Common;

namespace ShankarAgriMart.Domain.Entities;

public class Address : BaseEntity
{
    public int UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string AddressLine1 { get; set; } = string.Empty;

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Pincode { get; set; }

    public bool IsDefault { get; set; }

    public User User { get; set; } = null!;
}