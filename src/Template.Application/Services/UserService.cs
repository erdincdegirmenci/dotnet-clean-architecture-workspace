using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Application.DTOs;
using Template.Application.Interfaces;

namespace Template.Application.Services;

public class UserService : IUserService
{
    private static readonly List<UserDto> _users = new();

    public Task<UserDto?> GetByIdAsync(Guid id)
        => Task.FromResult(_users.FirstOrDefault(u => u.Id == id));

    public Task<IEnumerable<UserDto>> GetAllAsync()
        => Task.FromResult(_users.AsEnumerable());

    public Task<UserDto> CreateAsync(UserDto user)
    {
        user.Id = Guid.NewGuid();
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return Task.FromResult(false);
        _users.Remove(user);
        return Task.FromResult(true);
    }
} 