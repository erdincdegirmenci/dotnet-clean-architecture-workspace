using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = _userRepository.GetUserById(id);
        return user == null ? null : new UserDto { Id = user.Id, UserName = user.UserName, Email = user.Email, Role = user.Role };
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        // Gerçekçi senaryoda tüm kullanıcılar dönülür
        return new List<UserDto>();
    }

    public async Task<int> CreateAsync(UserDto userDto)
    {
        _passwordHasher.CreatePasswordHash(userDto.Password!, out var hash, out var salt);
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = userDto.UserName,
            Email = userDto.Email,
            Role = userDto.Role,
            PasswordHash = hash,
            PasswordSalt = salt
        };
        return _userRepository.CreateUser(user);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        // Gerçekçi senaryoda kullanıcı silme işlemi yapılır
        return false;
    }

    public async Task<User?> ValidateUserAsync(string userName, string password)
    {
        var user = _userRepository.GetUserByUserName(userName);
        if (user == null) return null;
        if (!_passwordHasher.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            return null;
        return user;
    }
} 