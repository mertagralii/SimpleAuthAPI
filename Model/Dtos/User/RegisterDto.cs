using System.ComponentModel.DataAnnotations;

namespace SimpleAuthAPI.Model.Dtos.User;

public class RegisterDto // Kullanıcıya gösterilecek olan kayıt Dtoları
{
    [Required]
    public string FirstName { get; set; } // Kullanıcı Adı Kısmı
    [Required]
    public string LastName { get; set; } // Kullanıcı Soy adı kısmı
    [Required]
    [EmailAddress]
    public string Email { get; set; } // Kullanıcının eposta adresi
    [Required]
    public string Password { get; set; } // Kullanıcının Şifresi
}