using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Repositories;

public class InMemoryUserRepository : IInMemoryUserRepository
{
    private static readonly List<User> _users = new();

    public Task<User?> GetByUserName(string userName)
        => Task.FromResult(_users.FirstOrDefault(u => u.UserName == userName));

    public Task<User?> GetById(Guid id)
        => Task.FromResult(_users.FirstOrDefault(u => u.Id == id));

    public Task Add(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }
} 