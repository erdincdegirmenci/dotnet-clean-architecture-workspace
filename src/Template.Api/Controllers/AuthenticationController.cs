using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Identity.Services;
using Template.Shared;
using Template.Domain.Entities;

namespace Template.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private static readonly Dictionary<string, string> _refreshTokens = new(); // refreshToken, userName

    private readonly IUserService _userService;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationController(IUserService userService, JwtTokenGenerator jwtTokenGenerator)
    {
        _userService = userService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<object>>> Login([FromBody] LoginRequestDto request)
    {
        var user = await _userService.ValidateUserAsync(request.UserName, request.Password);
        if (user == null)
            return Unauthorized(ApiResponse<object>.FailResponse("Invalid credentials"));

        var accessToken = _jwtTokenGenerator.GenerateToken(user);
        var refreshToken = Guid.NewGuid().ToString();
        _refreshTokens[refreshToken] = user.UserName;
        return Ok(ApiResponse<object>.SuccessResponse(new { accessToken, refreshToken }));
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] UserDto userDto)
    {
        var created = await _userService.CreateAsync(userDto);
        return Ok(ApiResponse<UserDto>.SuccessResponse(created, "User registered"));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<object>>> Refresh([FromBody] RefreshRequestDto request)
    {
        if (!_refreshTokens.TryGetValue(request.RefreshToken, out var userName))
            return Unauthorized(ApiResponse<object>.FailResponse("Invalid refresh token"));
        var user = await _userService.GetByIdAsync(Guid.Empty); // Kullanıcıyı bulmak için UserService kullanılabilir
        if (user == null)
            return Unauthorized(ApiResponse<object>.FailResponse("User not found"));
        var accessToken = _jwtTokenGenerator.GenerateToken(new User { UserName = user.UserName, Email = user.Email, Role = user.Role, Id = user.Id });
        var newRefreshToken = Guid.NewGuid().ToString();
        _refreshTokens.Remove(request.RefreshToken);
        _refreshTokens[newRefreshToken] = user.UserName;
        return Ok(ApiResponse<object>.SuccessResponse(new { accessToken, refreshToken = newRefreshToken }));
    }

    [HttpPost("logout")]
    [Authorize]
    public ActionResult<ApiResponse<bool>> Logout([FromBody] RefreshRequestDto request)
    {
        if (_refreshTokens.ContainsKey(request.RefreshToken))
            _refreshTokens.Remove(request.RefreshToken);
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Logged out"));
    }
} 