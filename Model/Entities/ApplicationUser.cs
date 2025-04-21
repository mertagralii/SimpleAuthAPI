using Microsoft.AspNetCore.Identity;

namespace SimpleAuthAPI.Model.Entities;

public class ApplicationUser : IdentityUser // IdentityUser'den kalıtım yapıyoruz, IdentityUser'e yeni özellikler ekliyoruz.
{
    public string FirstName { get; set; } // Kullanıcımızın artık ismi var
    
    public string LastName { get; set; } // Kullanıcımızın artık soy ismi var
    
    public DateTime CreatedDate { get; set; } = DateTime.Now; // Kullanıcımızın artık hesap oluşturma tarihi var.
    
    // Kısaca IdentityUser'de bulunan kolonlara yeni özellikler ekleyerek özelleştirdik.
}