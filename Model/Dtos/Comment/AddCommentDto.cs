namespace SimpleAuthAPI.Model.Dtos.Comment;

public class AddCommentDto
{
    public int TweetId { get; set; }
    public string Text { get; set; }
}