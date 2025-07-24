using System;
using System.Threading.Tasks;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Application.Services;
using Xunit;

namespace Template.Tests;

public class UserServiceTests
{
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _userService = new UserService();
    }

    [Fact]
    public async Task CreateUser_ShouldReturnUserWithId()
    {
        var userDto = new UserDto { UserName = "testuser", Email = "test@example.com" };
        var created = await _userService.CreateAsync(userDto);
        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal("testuser", created.UserName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenUserNotFound()
    {
        var user = await _userService.GetByIdAsync(Guid.NewGuid());
        Assert.Null(user);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenUserNotFound()
    {
        var result = await _userService.DeleteAsync(Guid.NewGuid());
        Assert.False(result);
    }
} 