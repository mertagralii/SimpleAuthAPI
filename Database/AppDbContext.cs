using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleAuthAPI.Model.Entities;

namespace SimpleAuthAPI.Database;

public class AppDbContext : IdentityDbContext<ApplicationUser> // Normalde IdentityUser vardı ama biz user kısmını özelleştirdik artık ApplicationUser classını kullanıyoruz.
{
    public DbSet<Tweet> Tweets { get; set; } // Tweets tablosu
    public DbSet<Comment> Comments { get; set; } // Comment tablosu
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        
    }
}