using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ModelStore.Application.Services;

namespace ModelStore.API.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost(ApiEndpoints.Users.Get)]
        public async Task<IActionResult> Get([FromBody] LoginRequest request, CancellationToken token)
        {
            var user = await _userService.SignInUser(request.Email, request.Password);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}