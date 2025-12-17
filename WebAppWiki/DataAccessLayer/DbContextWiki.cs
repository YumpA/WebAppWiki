using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAppWiki.Authorize;
using WebAppWiki.Domains;

namespace WebAppWiki.DataAccessLayer
{
    public class DbContextWiki : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public DbSet<Game> Games { get; set; }

        public DbSet<Charachters> Charachters { get; set; }
        
        public DbSet<Fractions> Fractions { get; set; }
        
        public DbSet<Genre> Genres { get; set; }
        
        public DbSet<Rating> Ratings { get; set; }
        
        public DbSet<Setting> Settings { get; set; }
        
        public DbSet<World> Worlds { get; set; }

        public DbContextWiki(DbContextOptions options) : base(options) { }
    }
}
