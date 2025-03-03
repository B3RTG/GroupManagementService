using Authentication.Application.Abstracts;
using Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthenticationDBContext _authenticationDBContext;

        public UserRepository(AuthenticationDBContext authenticationDBContext)
        {
            this._authenticationDBContext = authenticationDBContext;
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            var user = await _authenticationDBContext.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

            return user;
        }

    }
}
