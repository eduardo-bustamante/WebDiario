using Microsoft.AspNetCore.Mvc;

namespace WebDiario.Controllers;

public class HomeController : Controller
{
    // GET: / ou /Home/Index
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}