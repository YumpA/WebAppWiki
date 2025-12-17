using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAppWiki.Abstract;
using WebAppWiki.BusinessLogic.Filters;
using WebAppWiki.BusinessLogic;
using WebAppWiki.Domains;
using WebAppWiki.Helpers;
using WebAppWiki.Models;

namespace WebAppWiki.Controllers
{
    public class HomeController : Controller
    {
        private readonly ServiceFilter _serviceFilter;
        private readonly ILogger<HomeController> _logger;

        private readonly IRepository<Game, long> _repoGames;
        private readonly IRepository<Genre, long> _repoGenres;

        public HomeController(ServiceFilter serviceFilter, ILogger<HomeController> logger)
        {
            _serviceFilter = serviceFilter;
            _logger = logger;
            
        }

        public IActionResult Index()
        {
            var filter = HttpContext.Session
                .LoadObject<FilterMain>();

            var model = _serviceFilter
                .ProjectToModel(filter);

            return View(model);
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
}
