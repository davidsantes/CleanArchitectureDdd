namespace CleanArchitecture.Domain.Users;

public sealed class UserRole
{
    public int RoleId { get; set; }
    public UserId? UserId { get; set; }
}
