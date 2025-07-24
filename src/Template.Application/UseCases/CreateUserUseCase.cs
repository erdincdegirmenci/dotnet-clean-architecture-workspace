using Template.Application.DTOs;
using Template.Application.Interfaces;

namespace Template.Application.UseCases;

public class CreateUserUseCase
{
    private readonly IUserService _userService;
    public CreateUserUseCase(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserDto> ExecuteAsync(UserDto userDto)
    {
        // Burada validasyon, iş kuralları vs. eklenebilir
        return await _userService.CreateAsync(userDto);
    }
} 