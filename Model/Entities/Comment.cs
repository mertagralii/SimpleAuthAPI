namespace SimpleAuthAPI.Model.Entities;

public class Comment
{
    public int Id { get; set; } // Yorumların Id'si
    public string UserId { get; set; } // Yorum atan Kullanıcının Id'si
    public ApplicationUser User { get; set; } // Yorum atan Kullanıcının Id'sinin bilgilerini getirsin.
    public int TweetId { get; set; } // Yorum atılan Tweet'in Id'si
    public Tweet Tweet { get; set; } // Yorum atılan Tweet'in Id'sinin bilgierini getiricek.
    public string Text { get; set; } // yorum atılan tweet'in bilgileri getirilsin
    public DateTime CreatedDate { get; set; } = DateTime.Now; // Yorumun atılma tarihi
    public DateTime ModifiedDate { get; set; } // yorumun güncellenme tarihi
}