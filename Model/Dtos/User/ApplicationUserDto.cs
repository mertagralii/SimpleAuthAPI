namespace SimpleAuthAPI.Model.Dtos.User;

public class ApplicationUserDto
{
    public string Id { get; set; } // User Id'si
    public string FirstName { get; set; } // User Adı
    public string LastName { get; set; } // User Soyadı
    public string Email { get; set; } // User Emaili
}