using Microsoft.EntityFrameworkCore;
using WDC_F24.infrastructure.Data;


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Text;
using WDC_F24.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace WDC_F24.infrastructure.Data
{

    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } 
       
    }
}
