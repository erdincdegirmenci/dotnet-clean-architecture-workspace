using AutoMapper;
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
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = _userRepository.GetUserById(id);
        if (user == null) return null;

        var dto = _mapper.Map<UserDto>(user);
        return dto;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = _userRepository.GetAllUser();
        var dtos = _mapper.Map<IEnumerable<UserDto>>(users);
        return dtos;
    }

    public async Task<int> CreateAsync(UserDto userDto)
    {
        _passwordHasher.CreatePasswordHash(userDto.Password!, out var hash, out var salt);

        var user = _mapper.Map<User>(userDto);
        user.Id = Guid.NewGuid();
        user.PasswordHash = hash;
        user.PasswordSalt = salt;

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