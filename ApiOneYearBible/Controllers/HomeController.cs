using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ApiOneYearBible.Models;

namespace ApiOneYearBible.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBibleReadingsRepository _repo;

    public HomeController(ILogger<HomeController> logger, IBibleReadingsRepository repo)
    {
        _repo = repo;
        _logger = logger;
    }
    
    
    public IActionResult Index()
    {
        var bibleReadings = _repo.GetAllBibleReadings();
        return View(bibleReadings);
    }

   public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}