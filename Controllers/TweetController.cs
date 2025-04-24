using System.Net;
using System.Net.Mail;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleAuthAPI.Database;
using SimpleAuthAPI.Model.Dtos.Comment;
using SimpleAuthAPI.Model.Dtos.Tweet;
using SimpleAuthAPI.Model.Dtos.User;
using SimpleAuthAPI.Model.Entities;

namespace SimpleAuthAPI.Controllers;

// Tweet işlemlerinin yapılacağı Controller Kısmı
[ApiController]
[Route("[controller]")]
public class TweetController : ControllerBase
{
    #region Bağımlılık Kısımlar
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TweetController(UserManager<ApplicationUser> userManager, AppDbContext context, IMapper mapper)
    {
        _userManager = userManager;
        _context = context;
        _mapper = mapper;
    }
    #endregion
    
    #region Tweet Listeleme Kısmı
    
    [HttpGet("[action]")]
    public ActionResult<TweetDto[]> AllTweet()
    {
        var allTweets = _context.Tweets
            .Include(x=> x.Comments)
            .Include(x=> x.User)
            .ToArray();
        if (allTweets.Length == 0)
        {
            return NotFound("No tweets found");
        }
        var result = _mapper.Map<TweetDto[]>(allTweets);
        return Ok(result);
    }
    /*
     * Önemli Not
     * ActionResult<TweetDto[]> ile IAcitonResult arasındaki farklar
     * 
     * ActionResult<TweetDto[]> Kısmı
     * 
     * Avantajı: Swagger’da ve response’ta JSON modelin net olarak "TweetDto[]" olduğu gösterilir.
     * Hem Ok(...) hem NotFound(...) gibi HTTP response’ları, hem de data içeriği (TweetDto[]) döndürülebilir.
     * Kodun tip güvenliği yüksek. Derleyici TweetDto[] dışındaki şeyleri döndürmeni engeller.
     * Hem T (data) hem de HTTP sonucu dönebilir.
     * TweetDto[] tipinde dönüş garanti altındadır.
     * Swagger otomatik olarak dönüş tipini anlar.
     *
     * IAcitonResult Kısmı
     * 
     * Daha genel. İstersen Ok(), BadRequest(), File(), Redirect() gibi pek çok şey dönebilirsin.
     * Ancak dönüş tipi (TweetDto[], string, vs.) Swagger tarafında tam görünmeyebilir.
     * Result tipi runtime’da belli olur.
     * Yalnızca HTTP sonucu (veri tipi belirtilmez).
     * Her türlü dönüşe açık (tip güvenliği yok).
     * Swagger’da dönüş tipi belirsiz olabilir.
     * Daha esnek ama daha az güvenli.
     */
    
    #endregion

    #region Tweet Getirme Kısmı

    [HttpGet("[action]")]
    public ActionResult<TweetDto[]> GetTweets(int id)
    {
        var selectedTweet = _context.Tweets
            .Include(x=> x.User)
            .Include(x=> x.Comments)
            .FirstOrDefault(x=>x.Id == id);
        if (selectedTweet == null)
        {
            return NotFound("Tweet not found");
        }
        var result = _mapper.Map<TweetDto>(selectedTweet);
        return Ok(result);
    }

    #endregion

    #region Tweet Atma Kısmı
    [Authorize]
    [HttpPost("[action]")]
    public ActionResult<AddTweetDto> AddTweet(AddTweetDto addTweetDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = _userManager.GetUserId(User);
        var addedTweet = _mapper.Map<Tweet>(addTweetDto);
        addedTweet.UserId = userId;
        _context.Tweets.Add(addedTweet);
        _context.SaveChanges();
        var result = _mapper.Map<TweetDto>(addedTweet);
        return Ok(result);
    }

    #endregion

    #region Tweet Güncelleme Kısmı
    
    [Authorize]
    [HttpPut("[action]")]
    public ActionResult<TweetDto> UpdateTweet(UpdateTweetDto updateTweetDto)
    {
        var selectedTweet = _context.Tweets.FirstOrDefault(x=>x.Id == updateTweetDto.Id);
        if (selectedTweet == null)
        {
            return NotFound("Tweet not found");
        }
        var userId = _userManager.GetUserId(User);
        if (selectedTweet.UserId != userId)
        {
            return Unauthorized();
        }
        selectedTweet.Text = updateTweetDto.Text;
        var results = _mapper.Map<Tweet>(selectedTweet);
        results.ModifiedDate = DateTime.Now;
        _context.SaveChanges();
        var result = _mapper.Map<TweetDto>(selectedTweet);
        return Ok(result);
    }

    #endregion

    #region Tweet Silme Kısmı
    [Authorize]
    [HttpDelete("[action]")]
    public ActionResult<TweetDto> DeleteTweet(int id)
    {
        var selectedTweet = _context.Tweets.FirstOrDefault(x=>x.Id == id);
        if (selectedTweet == null)
        {
            return NotFound("Tweet not found");
        }
        var userId = _userManager.GetUserId(User);
        if (selectedTweet.UserId != userId)
        {
            return Unauthorized();
        }
        _context.Tweets.Remove(selectedTweet);
        _context.SaveChanges();
        var result = _mapper.Map<TweetDto>(selectedTweet);
        return Ok(result);
       
    }

    #endregion

    #region Tweet'e yorum atma Kısmı
    
    [Authorize]
    [HttpPost("[action]")]
    public ActionResult<AddCommentDto> AddComment(AddCommentDto addCommentDto)
    {
        var userId = _userManager.GetUserId(User);
        var addedComment = _mapper.Map<Comment>(addCommentDto);
        addedComment.UserId = userId;
        _context.Comments.Add(addedComment);
        _context.SaveChanges();
        var result = _mapper.Map<Comment>(addedComment);
        return Ok(result);
    }

    #endregion

    #region Mail Gönderme Kısmı 
    [Authorize]
    [HttpPost("[action]")]
    public async Task<ActionResult> SendEmail(ApplicationUserDto applicationUserDto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }
        var client = new SmtpClient();
        client.Host = "smtp.resend.com";
        client.Port = 587;
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential("resend", "re_fHuYHaVw_CLEvwj2VFHq7A6UfiqMMnkFh");
        var message = new MailMessage();
        message.To.Add(new MailAddress($"{applicationUserDto.Email}", $"{applicationUserDto.FirstName}{applicationUserDto.LastName}"));
        message.From = new MailAddress("noreply@bildirim.akademiprojeler.com", "Akademi Projeler");
        message.Subject = $"Merhaba {applicationUserDto.FirstName} {applicationUserDto.LastName} ";
        message.Body = "Mesaj İçeriği";
        try
        {
            await client.SendMailAsync(message);

        }
        catch
        {

        }

        message.Dispose();
        client.Dispose();
        return Ok();
    }

    #endregion
    
    
}