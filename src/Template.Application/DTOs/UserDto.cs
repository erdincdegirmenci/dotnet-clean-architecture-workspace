namespace Template.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; }
    public List<string> Permissions { get; set; }
    public string? Password { get; set; } // Sadece create/register için
} 