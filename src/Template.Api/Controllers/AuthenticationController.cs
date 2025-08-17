using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Api.Common;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Domain.Entities;
using Template.Infrastructure.Helper;
using Template.Infrastructure.Security;

namespace Template.Api.Controllers;

[Route("api/[controller]")]
public class AuthenticationController : BaseController
{
    private static readonly Dictionary<string, string> _refreshTokens = new(); 

    private readonly IUserService _userService;
    private readonly IJwtTokenHandler _jwtTokenHandler;
    private readonly ILogger<AuthenticationController> _logManager;
    private readonly JwtHelper _jwtHelper;

    public AuthenticationController(IUserService userService, IJwtTokenHandler jwtTokenGenerator, ILogger<AuthenticationController> logManager, JwtHelper jwtHelper)
    {
        _userService = userService;
        _jwtTokenHandler = jwtTokenGenerator;
        _logManager = logManager;
        _jwtHelper = jwtHelper;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<object>>> Login([FromBody] LoginRequestDto request)
    {
        //var user = await _userService.ValidateUserAsync(request.UserName, request.Password);
        var user = new UserDto
        {
            Id = Guid.NewGuid(),
            UserName = "admin",
            Email = "admin@mail.com",
            Roles = new List<string> { "Admin" },
            Permissions = new List<string> { "Read", "Write" },
            Password = "admin" // sadece demo veya register için
        };
        if (user == null)
        {
            _logManager.LogWarning("Login attempt failed for username: {UserName} - Invalid credentials", request.UserName);
            return Unauthorized(ApiResponse<object>.FailResponse("Invalid credentials"));
        }

        var accessToken = _jwtTokenHandler.GenerateAccessToken(user.Id.ToString(), user.Roles, _jwtHelper.GetJwtOptions());
        var refreshToken = Guid.NewGuid().ToString();
        _refreshTokens[refreshToken] = user.UserName;
        return Ok(ApiResponse<object>.SuccessResponse(new { accessToken, refreshToken }));
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] UserDto userDto)
    {
        var created = await _userService.CreateAsync(userDto);
        return Ok(ApiResponse<int>.SuccessResponse(created, "User registered"));
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
        var accessToken = _jwtTokenHandler.GenerateAccessToken(user.Id.ToString(), user.Roles, _jwtHelper.GetJwtOptions());
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