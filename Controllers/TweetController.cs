using Microsoft.AspNetCore.Mvc;

namespace SimpleAuthAPI.Controllers;

public class TweetController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}