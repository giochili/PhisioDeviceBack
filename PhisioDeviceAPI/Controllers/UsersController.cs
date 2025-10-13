using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhisioDeviceAPI.DTOs.User;
using PhisioDeviceAPI.DTOs.Auth;
using PhisioDeviceAPI.Repository;
using PhisioDeviceAPI.Service;

namespace PhisioDeviceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO request, CancellationToken cancellationToken)
        {
            try
            {
                var created = await _userService.RegisterAsync(request, cancellationToken);
                return CreatedAtAction(nameof(Register), new { id = created.Id }, new { created.Id, created.Name, created.Email });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _userService.LoginAsync(request.Email, request.Password, cancellationToken);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            try
            {
                return Ok(new
                {
                    userId = User.FindFirst("userId")?.Value,
                    email = User.FindFirst("email")?.Value,
                    name = User.FindFirst("name")?.Value,
                    role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}


