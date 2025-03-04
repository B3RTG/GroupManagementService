using Authentication.Application.Abstracts;
using Authentication.Domain.Entities;
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

        /// <summary>
        /// Returns a token for the user
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<UserToken> LoginAsync(LoginRequest loginRequest)
        {
            var userToken = await _accountService.LoginAsync(loginRequest);
            return userToken;
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(string refreshToken)
        {
            var userToken = await _accountService.RefreshTokenAsync(refreshToken);
            return Ok(userToken);
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
