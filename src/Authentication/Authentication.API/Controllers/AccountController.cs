using Authentication.Application.Abstracts;
using Authentication.Domain.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="registerRequest">Data to use for the resgister</param>
        /// <returns>Http 200 if request is proccessed correctly</returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest registerRequest)
        {
            await _accountService.RegisterAsync(registerRequest);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
        {
            var (token, expirationDate) = await _accountService.LoginAsync(loginRequest);
            return Ok(new { token, expirationDate });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(string refreshToken)
        {
            var (token, expirationDate) = await _accountService.RefreshTokenAsync(refreshToken);
            return Ok(new { token, expirationDate });
        }

        [HttpPost("login-cookie")]
        public async Task<IActionResult> LoginWithCookieAsync(LoginRequest loginRequest)
        {
            await _accountService.LoginWithCookieAsync(loginRequest);
            return Ok();
        }

        [HttpPost("refresh-token-cookie")]
        public async Task<IActionResult> RefreshTokenWithCookiesAsync(string refreshToken)
        {
            await _accountService.RefreshTokenWithCookiesAsync(refreshToken);
            return Ok();
        }
    }
}
