using Humanizer.Localisation;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppWiki.Abstract;
using WebAppWiki.Authorize;
using WebAppWiki.BusinessLogic;
using WebAppWiki.BusinessLogic.Filters;
using WebAppWiki.Domains;
using WebAppWiki.Helpers;
using WebAppWiki.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebAppWiki.Controllers
{
    [Authorize(Roles = $"{AuthConstants.Roles.Admin}")]
    public class AdminController : Controller
    {
		private readonly ServiceAdmin _admin;
		private readonly IMapper _mapper;
        private readonly IRepository<Genre, long> _repoGenres;
        private readonly IRepository<Fractions, long> _repoFractions;
        private readonly IRepository<World, long> _repoWorld;
        private readonly IRepository<Rating, long> _repoRating;
        private readonly IRepository<Game, long> _repoGames;
        private readonly IRepository<Setting, long> _repoSettings;
        private readonly IRepository<Charachters, long> _repoCharachters;

        public AdminController(ServiceAdmin admin, IMapper mapper,
            IRepository<Genre, long> repoGenres, IRepository<Fractions, long> repoFractions,
            IRepository<World, long> repoWorld, IRepository<Rating, long> repoRating,
            IRepository<Game, long> repoGames, IRepository<Setting, long> repoSettings,
            IRepository<Charachters, long> repoCharachters)
        {
			_admin = admin;
			_mapper = mapper;
            _repoGenres = repoGenres;
            _repoFractions = repoFractions;
            _repoWorld = repoWorld;
            _repoRating = repoRating;
            _repoGames = repoGames;
            _repoSettings = repoSettings;
            _repoCharachters = repoCharachters;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GamesList()
        {
			var games = _repoGames.GetAll().Include("Rating").Include("Genre");
            
			return View(games);
        }

        public IActionResult GenreList()
        {
            var genres = _repoGenres.GetAll();

            return View(genres);
        }

        public IActionResult FractionsList()
        {
            var fractions = _repoFractions.GetAll();

            return View(fractions);
        }

		public IActionResult WorldList()
		{
			var worlds = _repoWorld.GetAll();

			return View(worlds);
		}

        public IActionResult CharachtersList()
        {
            var worlds = _repoCharachters.GetAll();

            return View(worlds);
        }

        public IActionResult ManageContentView()
        {
            return View(nameof(ManageContentView));
        }

        public IActionResult ManageRolesView()
        {
            var modelList = _admin.GetRoleModels();

            return View(modelList);
        }

        [HttpPost]
        public async Task<IActionResult> EditRolesView(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                await _admin.UpdateRoleAsync(model);
                string url = Url.Action("Index", "Admin");
                return Redirect(url);
            }

            return View(model);
        }

        public IActionResult CreateGameView()
        {
            GameEditModel model = _admin.CreateGameEditModel();
            model.ReturnUrl = Request.Headers["Referer"];

            return View("GameEditView", model);
        }

		public IActionResult CreateGenreView()
        {
            var model = new GenreEditModel();

            return View(nameof(EditGenreView), model);
        }

        public IActionResult CreateFractionView()
        {
            FractionEditModel model = _admin.CreateFractionEditModel();

            return View(nameof(EditFractionsView), model);
        }

        public IActionResult CreateWorldView()
        {
            var model = _admin.CreateWorldEditModel();

            return View(nameof(EditWorldView), model);
        }

		public IActionResult CreateCharachtersView()
		{
			var model = _admin.CreateCharachtersEditModel();

			return View(nameof(EditCharachtersView), model);
		}

		public IActionResult CreateRating()
        {
            var model = new RatingEditModel();

            return View(nameof(EditRatingView), model);
        }

        [HttpPost]
        public IActionResult EditRatingView(RatingEditModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Rating>(model);

                if (entity.RatingId == 0)
                    _repoRating.Create(entity);
                else
                    _repoRating.Update(entity);

                string url = Url.Action("Index", "Admin");

                return Redirect(url);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult EditWorldView(WorldEditModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<World>(model);

                if (entity.WorldId == 0)
                    _admin.CreateWorld(model);
                else
                    _repoWorld.Update(entity);

                string url = Url.Action("Index", "Admin");

                return Redirect(url);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult EditFractionsView(FractionEditModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Fractions>(model);

                if (entity.FractionsId == 0)
                    _admin.CreateFraction(model);
                else
                    _repoFractions.Update(entity);

                string url = Url.Action("Index", "Admin");

                return Redirect(url);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult EditCharachtersView(CharachtersEditModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Charachters>(model);

                if (entity.CharachtersId == 0)
                    _admin.CreateCharachters(model);
                else
                    _repoCharachters.Update(entity);

                string url = Url.Action("Index", "Admin");

                return Redirect(url);
            }

            return View(model);
        }

		[HttpPost]
        public IActionResult EditGenreView(GenreEditModel model)
        {
            if(ModelState.IsValid)
            {
                var entity = _mapper.Map<Genre>(model);

                if (entity.GenreId == 0)
                    _repoGenres.Create(entity);
                else
                    _repoGenres.Update(entity);

                string url = Url.Action("Index", "Admin");

                return Redirect(url);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult GameEditView(GameEditModel model)
        {
            if (ModelState.ErrorCount > 0)
            {
                var errors = ModelState.Values.Select(e => e.Errors).ToList();
            }

            if (ModelState.IsValid)
            {
                if (model.GameId == 0)
                    _admin.CreateGame(User, model);
                else
                    _admin.UpdateGame(User, model);

				string? returnUrl = model.ReturnUrl == null ?
				   Url.Action("List", "Games") :
				   model.ReturnUrl;

				return Redirect(returnUrl);
			}

            return View(model);
        }

        public IActionResult GameEditView(long id)
        {
            var model = _admin.GetGameEditModel(id);
            model.ReturnUrl = Request.Headers["Referer"];

            return View(model);
        }

        public IActionResult EditGenreView(long id)
        {
            var model = _admin.GetGenreEditModel(id);
            model.ReturnUrl = Request.Headers["Referer"];

            return View(model);
        }

        public IActionResult EditFractionsView(long id)
        {
            var model = _admin.GetFractionsEditModel(id);
            model.ReturnUrl = Request.Headers["Referer"];

            return View(model);
        }

		public IActionResult EditWorldView(long id)
		{
			var model = _admin.GetWorldEditModel(id);
			model.ReturnUrl = Request.Headers["Referer"];

			return View(model);
		}

        public IActionResult EditCharachtersView(long id)
        {
            var model = _admin.GetCharachtersEditModel(id);
            model.ReturnUrl = Request.Headers["Referer"];

            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteGame(long id)
        {
            var game =_repoGames.Read(id);
            if (game == null)
                return NotFound();
            var model = _admin.GetGameEditModel(id);

            _admin.DeleteGame(model, game);
            return RedirectToAction(nameof(GamesList));
        }

        [HttpPost]
        public IActionResult DeleteGenre(long id)
        {
            var genre = _repoGenres.Read(id);
            if (genre == null)
                return NotFound();

            _repoGenres.Delete(id);
            return RedirectToAction(nameof(GenreList));
        }

        [HttpPost]
        public IActionResult DeleteFractions(long id)
        {
            var fraction = _repoFractions.Read(id);
            if (fraction == null)
                return NotFound();

            _admin.DeleteFractions(fraction);
            return RedirectToAction(nameof(FractionsList));
        }

		[HttpPost]
		public IActionResult DeleteWorld(long id)
		{
			var world = _repoWorld.Read(id);
			if (world == null)
				return NotFound();

			_admin.DeleteWorld(world);
			return RedirectToAction(nameof(WorldList));
		}

        [HttpPost]
        public IActionResult DeleteCharachters(long id)
        {
            var charachters = _repoCharachters.Read(id);
            if (charachters == null)
                return NotFound();

            _admin.DeleteCharachters(charachters);
            return RedirectToAction(nameof(WorldList));
        }
    }
}
