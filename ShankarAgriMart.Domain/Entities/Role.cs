using ShankarAgriMart.Domain.Common;

namespace ShankarAgriMart.Domain.Entities;

public class Role : BaseEntity
{
    public string RoleName { get; set; } = string.Empty;

    public ICollection<User> Users { get; set; } = new List<User>();
}