using Microsoft.EntityFrameworkCore;
using testproject.Configuration;
using testproject.Entities;

namespace testproject
{
    public class ApplicationContext:DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }    
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new GameConfiguration());
        }
    }
}
