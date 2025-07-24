using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Domain.Entities;
using Template.Identity.Services;
using Template.Shared;

namespace Template.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public UsersController(IUserService userService, JwtTokenGenerator jwtTokenGenerator)
    {
        _userService = userService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<UserDto>>.SuccessReponse(users));
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound(ApiResponse<UserDto>.FailResponse("User not found"));
        return Ok(ApiResponse<UserDto>.SuccessReponse(user));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<UserDto>>> Create(UserDto userDto)
    {
        var created = await _userService.CreateAsync(userDto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, ApiResponse<UserDto>.SuccessReponse(created));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
    {
        var result = await _userService.DeleteAsync(id);
        if (!result)
            return NotFound(ApiResponse<bool>.FailResponse("User not found"));
        return Ok(ApiResponse<bool>.SuccessReponse(true));
    }

    [HttpPost("token")]
    [AllowAnonymous]
    public ActionResult<ApiResponse<string>> GenerateToken([FromBody] UserDto userDto)
    {
        // Demo amaçlı: UserDto'dan User entity'ye dönüştürüp token üretiyoruz
        var user = new User
        {
            Id = userDto.Id == Guid.Empty ? Guid.NewGuid() : userDto.Id,
            UserName = userDto.UserName,
            Email = userDto.Email,
            Role = userDto.Role ?? "User"
        };
        var token = _jwtTokenGenerator.GenerateToken(user);
        return Ok(ApiResponse<string>.SuccessReponse(token));
    }
} 