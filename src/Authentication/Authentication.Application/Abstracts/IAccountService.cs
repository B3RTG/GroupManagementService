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


        Task<(string token, DateTime expirationDate)> LoginAsync(LoginRequest loginRequest);
        Task<(string token, DateTime expirationDate)> RefreshTokenAsync(string? refreshToken);

    }
}
