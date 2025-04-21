using SimpleAuthAPI.Model.Dtos.Comment;
using SimpleAuthAPI.Model.Dtos.User;

namespace SimpleAuthAPI.Model.Dtos.Tweet;

public class TweetDto
{
    public int Id { get; set; } // Tweet Id'si
    public string Text { get; set; } // Tweet Yazısı
    public ApplicationUserDto User { get; set; } // Tweet'i atan kullanıcı bilgileri
    public DateTime CreatedDate { get; set; } // Tweet'in oluşturulma tarihi
    public DateTime ModifiedDate { get; set; } // Tweet'in güncellenme tarihi
    public ICollection<CommentDto> Comments { get; set; } // Tweet'e ait yorum bilgileri
    
}