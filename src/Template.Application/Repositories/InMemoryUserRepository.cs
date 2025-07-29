using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private static readonly List<User> _users = new();

    public Task<User?> GetByUserNameAsync(string userName)
        => Task.FromResult(_users.FirstOrDefault(u => u.UserName == userName));

    public Task<User?> GetByIdAsync(Guid id)
        => Task.FromResult(_users.FirstOrDefault(u => u.Id == id));

    public Task AddAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }
} 