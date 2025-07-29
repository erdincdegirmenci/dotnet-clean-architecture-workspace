using Template.Domain.Entities;

namespace Template.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUserNameAsync(string userName);
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    // Diğer user işlemleri eklenebilir
} 