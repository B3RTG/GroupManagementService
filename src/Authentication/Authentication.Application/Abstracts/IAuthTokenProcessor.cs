using Authentication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Abstracts
{
    public interface IAuthTokenProcessor
    {
        public UserToken GenerateJwtToken(User user);
        public string GenerateRefreshToken();
        public void WriteAuthenticationTokenAsHttpCookie(string cookieName, string token, DateTime exporation);
    }
}
