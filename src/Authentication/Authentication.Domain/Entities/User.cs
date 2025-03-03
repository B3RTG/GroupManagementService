using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTimeAtUtc { get; set; }

        public User(string email)
        {
            Email = email;
            UserName = email;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }



    }
}
