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

    protected override void OnModelCreating(ModelBuilder builder) // Veritabanı ayalarını buradan yapıyorum
    {
        base.OnModelCreating(builder);
        
        // Kullanıcı silinirse Tweet'leri silinsin
        builder.Entity<Tweet>()
            .HasOne(t => t.User) // Tweet, bir ApplicationUser'a (kullanıcıya) sahiptir
            .WithMany() // ApplicationUser birden fazla Tweet'e sahip olabilir
            .HasForeignKey(t => t.UserId) // Tweet'in UserId'si, ApplicationUser tablosundaki Id'yi referans alır
            .OnDelete(DeleteBehavior.Cascade); // Eğer bir kullanıcı silinirse, bu kullanıcıya ait tüm tweet'ler de silinsin

        // Tweet silinirse yorumları da silinsin
        builder.Entity<Comment>()
            .HasOne(c => c.Tweet) // Comment, bir Tweet'e sahiptir
            .WithMany(t => t.Comments) // Tweet, birden fazla Comment'e (Yorum) sahip olabilir
            .HasForeignKey(c => c.TweetId) // Yorumun TweetId'si, Tweet tablosundaki Id'yi referans alır
            .OnDelete(DeleteBehavior.Cascade); // Eğer bir tweet silinirse, o tweet'e ait tüm yorumlar da silinsin

        // Kullanıcı silinse bile yorumlar Tweet üzerinden cascade silineceği için burada NoAction diyebiliriz
        builder.Entity<Comment>()
            .HasOne(c => c.User) // Yorum, bir kullanıcıya sahiptir
            .WithMany() // Kullanıcı birden fazla yorum yapabilir
            .HasForeignKey(c => c.UserId) // Yorumun UserId'si, ApplicationUser tablosundaki Id'yi referans alır
            .OnDelete(DeleteBehavior.NoAction); // Eğer kullanıcı silinirse, yorumlar silinmez çünkü silinme işlemi tweet üzerinden cascade ile yapılır

    }
}