using System;

namespace Authentication.Domain.Entities;

public class UserToken
{
    public string Token { get; set; } = null!;
    public DateTime Expiration { get; set; }
    public string RefreshToken { get; set; } = null!;
    public DateTime RefreshTokenExpiration { get; set; }
}
