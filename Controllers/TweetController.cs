using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleAuthAPI.Database;
using SimpleAuthAPI.Model.Dtos.Tweet;
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
        var allTweets = _context.Tweets.Include(x=> x.User)
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
}