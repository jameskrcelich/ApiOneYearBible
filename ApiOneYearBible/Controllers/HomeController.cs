using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ApiOneYearBible.Models;

namespace ApiOneYearBible.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBibleReadingsRepository repo;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    public HomeController(BibleReadingsRepository repo)
    {
        this.repo = repo;
    }
    
    public IActionResult Index()
    {
        var bibleReadings = repo.GetAllBibleReadings();
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