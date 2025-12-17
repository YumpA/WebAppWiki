using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using WebAppWiki.Abstract;
using WebAppWiki.BusinessLogic;
using WebAppWiki.BusinessLogic.Filters;
using WebAppWiki.Domains;
using WebAppWiki.Helpers;
using WebAppWiki.Models;

namespace WebAppWiki.Controllers
{	
	public class GamesController : Controller
	{
        private readonly ServiceGame _serviceGame;
        private readonly ServiceFilter _serviceFilter;
		private readonly IRepository<Game, long> _repoGames;
        private readonly IMapper _mapper;

        public GamesController(ServiceGame serviceGame, ServiceFilter serviceFilter, IRepository<Game, long> repoGames, IMapper mapper)
        {
            _serviceGame = serviceGame;
            _serviceFilter = serviceFilter;
			_repoGames = repoGames;
            _mapper = mapper;
        }

        public IActionResult Index()
		{
			return View();
		}

		public IActionResult List()
		{
			var filter = HttpContext.Session
				.LoadObject<FilterMain>(isCreateNew: false);

			var games = _repoGames.GetAll();

			if (filter != null)
			{
				games = _serviceFilter.ApplyFilter(filter);
			}

			return View(games);
		}

		public IActionResult Details(long id)
		{
			var model = _serviceGame.GetGameDetailsModel(id);
            model.ReturnUrl = Request.Headers["Referer"];

            return View(model);
        }
	}
}
