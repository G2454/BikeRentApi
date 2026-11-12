using RentBikeApi.Domain.Enums;

namespace RentBikeApi.Domain.Entities;
public class User
{
    public const int PasswordMinLength = 8;

    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;  
    public UserRole Role { get; set; }
    public string? CourierId { get; set; }                      
}