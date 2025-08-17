using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Api.Common;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Infrastructure.Security;

namespace Template.Api.Controllers;

[Route("api/[controller]")]
public class UsersController : BaseController
{
    private readonly IUserService _userService;
    private readonly IJwtTokenHandler _jwtTokenHandler;

    public UsersController(IUserService userService, IJwtTokenHandler jwtTokenGenerator)
    {
        _userService = userService;
        _jwtTokenHandler = jwtTokenGenerator;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<UserDto>>.SuccessResponse(users));
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound(ApiResponse<UserDto>.FailResponse("User not found"));
        return Ok(ApiResponse<UserDto>.SuccessResponse(user));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<UserDto>>> Create(UserDto userDto)
    {
        var created = await _userService.CreateAsync(userDto);
        return CreatedAtAction(nameof(GetById), new { id = created }, ApiResponse<int>.SuccessResponse(created));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
    {
        var result = await _userService.DeleteAsync(id);
        if (!result)
            return NotFound(ApiResponse<bool>.FailResponse("User not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }
} 