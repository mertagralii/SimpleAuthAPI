namespace SimpleAuthAPI.Model.Entities;

public class Tweet
{
    public int Id { get; set; } // Tweetlerin Id'si
    
    public string UserId { get; set; } // Kullanıcının Id'si
    
    public ApplicationUser User { get; set; } // Kullanıcının Id bilgilerini getirir.
    
    public string Text { get; set; } // Tweet yazisi

    public DateTime CreatedDate { get; set; } = DateTime.Now; // Tweet'in yazılma Tarihi
    
    public DateTime ModifiedDate { get; set; } // Tweet'in güncellenme tarihi

    public ICollection<Comment> Comments { get; set; } // Tweetlerin Yorumları
}