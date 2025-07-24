namespace Template.Application.Interfaces;

using Template.Application.DTOs;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> CreateAsync(UserDto user);
    Task<bool> DeleteAsync(Guid id);
} 