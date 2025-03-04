using Authentication.Application.Abstracts;
using Authentication.Domain.Entities;
using Authentication.Domain.Exceptions;
using Authentication.Domain.Request;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAuthTokenProcessor _authTokenProcessor;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public AccountService(IAuthTokenProcessor authTokenProcessor, UserManager<User> userManager, IUserRepository userRepository)
        {
            this._authTokenProcessor = authTokenProcessor;
            this._userManager = userManager;
            this._userRepository = userRepository;
        }

        public async Task RegisterAsync(RegisterRequest registerRequest)
        {
            bool userExist = await _userManager.FindByEmailAsync(registerRequest.Email) != null;

            if (userExist)
            {
                throw new UserAlreadyExistsException(registerRequest.Email);
            }

            var user = new User(registerRequest.Email)
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName
            };
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, registerRequest.Password);

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                throw new RegistrationFailedException(result.Errors.Select( x => x.Description));
            }
        }

        // LoginAsync method with cookie authentication
        public async Task LoginWithCookieAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                throw new LoginFailedException(loginRequest.Email);
            }

            UserToken userToken = _authTokenProcessor.GenerateJwtToken(user);
            
            var refreshToken = _authTokenProcessor.GenerateRefreshToken();
            var refreshTokenExpirationDate = DateTime.UtcNow.AddDays(7); //put 7 days in configuration
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTimeAtUtc = refreshTokenExpirationDate;
            await _userManager.UpdateAsync(user);

            userToken.RefreshToken = refreshToken;
            userToken.RefreshTokenExpiration = refreshTokenExpirationDate;


            _authTokenProcessor.WriteAuthenticationTokenAsHttpCookie("access_token", userToken.Token, userToken.Expiration);
            _authTokenProcessor.WriteAuthenticationTokenAsHttpCookie("refresh_token", refreshToken, refreshTokenExpirationDate);
        }

        // LoginAsync method without cookie authentication
        public async Task<UserToken> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                throw new LoginFailedException(loginRequest.Email);
            }
            UserToken userToken = _authTokenProcessor.GenerateJwtToken(user);

            var refreshToken = _authTokenProcessor.GenerateRefreshToken();
            var refreshTokenExpirationDate = DateTime.UtcNow.AddDays(7); //put 7 days in configuration
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTimeAtUtc = refreshTokenExpirationDate;
            await _userManager.UpdateAsync(user);

            userToken.RefreshToken = refreshToken;
            userToken.RefreshTokenExpiration = refreshTokenExpirationDate;

            return userToken;
        }

        // RefreshTokenAsync method with cookie authentication
        public async Task RefreshTokenWithCookiesAsync(string? refreshToken)
        {

            if(string.IsNullOrEmpty(refreshToken))
            {
                throw new InvalidRefreshTokenException("RefreshToken not found");
            }

            var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken) ?? throw new InvalidRefreshTokenException("Unable to retrieve user from RefreshToken");

            if(user.RefreshTokenExpiryTimeAtUtc < DateTime.UtcNow)
            {
                throw new InvalidRefreshTokenException("RefreshToken expired");
            }


            UserToken userToken = _authTokenProcessor.GenerateJwtToken(user);

            refreshToken = _authTokenProcessor.GenerateRefreshToken();
            var refreshTokenExpirationDate = DateTime.UtcNow.AddDays(7); //put 7 days in configuration
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTimeAtUtc = refreshTokenExpirationDate;
            await _userManager.UpdateAsync(user);

            userToken.RefreshToken = refreshToken;
            userToken.RefreshTokenExpiration = refreshTokenExpirationDate;

            _authTokenProcessor.WriteAuthenticationTokenAsHttpCookie("access_token", userToken.Token, userToken.Expiration);
            _authTokenProcessor.WriteAuthenticationTokenAsHttpCookie("refresh_token", refreshToken, refreshTokenExpirationDate);
        }


        // RefreshTokenAsync method without cookie authentication
        public async Task<UserToken> RefreshTokenAsync(string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new InvalidRefreshTokenException("RefreshToken not found");
            }
            var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken) ?? throw new InvalidRefreshTokenException("Unable to retrieve user from RefreshToken");
            if (user.RefreshTokenExpiryTimeAtUtc < DateTime.UtcNow)
            {
                throw new InvalidRefreshTokenException("RefreshToken expired");
            }

            UserToken userToken = _authTokenProcessor.GenerateJwtToken(user);
            refreshToken = _authTokenProcessor.GenerateRefreshToken();
            var refreshTokenExpirationDate = DateTime.UtcNow.AddDays(7); //put 7 days in configuration
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTimeAtUtc = refreshTokenExpirationDate;
            await _userManager.UpdateAsync(user);

            userToken.RefreshToken = refreshToken;
            userToken.RefreshTokenExpiration = refreshTokenExpirationDate;

            return userToken;
        }

    }
}
