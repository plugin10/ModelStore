using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ModelStore.Application;
using ModelStore.Application.Services;

namespace ModelStore.API.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenGenerator _tokenGenerator;

        public UserController(IUserService userService, TokenGenerator tokenGenerator)
        {
            _userService = userService;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost(ApiEndpoints.Users.Get)]
        public async Task<IActionResult> Get([FromBody] LoginRequest request, CancellationToken token)
        {
            var user = await _userService.SignInUserAsync(request.Email, request.Password);

            if (user == null)
            {
                return NotFound("Invalid email or password");
            }

            var jwtToken = _tokenGenerator.GenerateToken(user.Id, user.Name, user.Email, user.UserType);

            return Ok(new { Token = jwtToken });
        }
    }
}