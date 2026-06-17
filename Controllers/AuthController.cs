using BiblioSystem.Dtos;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;

        public AuthController(
            AuthService service
        )
        {
            _service =
                service;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult>
        Login(
            LoginDto data
        )
        {
            var token =
                await _service
                .Login(
                    data
                );

            return Ok(
                new
                {
                    token
                }
            );
        }
    }
}