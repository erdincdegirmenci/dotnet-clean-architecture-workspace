using Template.Domain.Entities;

namespace Template.Application.Interfaces;

public interface IInMemoryUserRepository
{
    Task<User?> GetByUserName(string userName);
    Task<User?> GetById(Guid id);
    Task Add(User user);
    // Diğer user işlemleri eklenebilir
} 