using SimpleAuthAPI.Model.Dtos.Tweet;
using SimpleAuthAPI.Model.Dtos.User;
using SimpleAuthAPI.Model.Entities;

namespace SimpleAuthAPI.Model.Dtos.Comment;

public class CommentDto
{
    public int Id { get; set; } // Yorumların Id'si
    public string Text { get; set; } // yorum atılan tweet'in bilgileri getirilsin
    public ApplicationUserDto User { get; set; } // Yorum atan Kullanıcının Id'sinin bilgilerini getirsin.
    public DateTime CreatedDate { get; set; } = DateTime.Now; // Yorumun atılma tarihi
    public DateTime ModifiedDate { get; set; } // yorumun güncellenme tarihi
    
}