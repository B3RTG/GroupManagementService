using Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Infrastructure
{
    public class AuthenticationDBContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AuthenticationDBContext(DbContextOptions<AuthenticationDBContext> options) : base(options)
        {
        }

        public override DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(256);
                entity.Property(e => e.LastName).HasMaxLength(256);
            });
        }
    }
}
