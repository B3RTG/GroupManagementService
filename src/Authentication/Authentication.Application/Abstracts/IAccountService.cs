using Authentication.Domain.Entities;
using Authentication.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Abstracts
{
    public interface IAccountService
    {
        Task RegisterAsync(RegisterRequest registerRequest);

        Task LoginWithCookieAsync(LoginRequest loginRequest);
        Task RefreshTokenWithCookiesAsync(string? refreshToken);


        Task<UserToken> LoginAsync(LoginRequest loginRequest);
        Task<UserToken> RefreshTokenAsync(string? refreshToken);

    }
}
