using AuthenticatorBackend.Services;
using Microsoft.AspNetCore.Mvc;
using AuthenticatorBackend.DTOs;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(UserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            _logger.LogWarning("Registration failed due to model state errors: {Errors}", string.Join(", ", errors));
            return BadRequest(new { Errors = errors });
        }

        try
        {
            bool success = await _userService.RegisterUser(request.Username, request.Password);
            if (success)
                return Ok("User registered successfully.");
            else
            {
                _logger.LogWarning("User already exist.");
                return BadRequest("User already exist.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while registering the user: {Username}", request.Username);
            return StatusCode(500, "An internal server error occurred.");
        }
    }

    [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
    if (!ModelState.IsValid)
    {
        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        _logger.LogWarning("Login failed due to model state errors: {Errors}", string.Join(", ", errors));
        return BadRequest(new { Errors = errors });
    }

    try
    {
        var (isValid, message) = await _userService.LoginUser(request.Username, request.Password);
        if (isValid)
            return Ok(message);
        else
        {
            _logger.LogWarning("Login failed: {Message}", message);
            return BadRequest(message);
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An error occurred while logging in the user: {Username}", request.Username);
        return StatusCode(500, "An internal server error occurred.");
    }
}

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        bool success = await _userService.ResetPassword(request.Username, request.NewPassword);

        if (success)
            return Ok("Password reset successfully.");
        return BadRequest("Password reset failed.");
    }
}

public class RegisterRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class LoginRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class ResetPasswordRequest
{
    public required string Username { get; set; }
    public required string NewPassword { get; set; }
}