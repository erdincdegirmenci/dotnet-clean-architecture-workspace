namespace Template.Application.Interfaces;

using Template.Application.DTOs;
using Template.Domain.Entities;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<int> CreateAsync(UserDto user);
    Task<bool> DeleteAsync(Guid id);
    Task<User?> ValidateUserAsync(string userName, string? password);
} 