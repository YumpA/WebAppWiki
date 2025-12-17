using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAppWiki.Authorize;
using WebAppWiki.DataAccessLayer;
using WebAppWiki.Domains;
using WebAppWiki.Models;

namespace WebAppWiki.BusinessLogic
{
    public class ServiceAdmin
    {
        private readonly DbContextWiki _context;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public ServiceAdmin(DbContextWiki context, IMapper mapper, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
        }

		//НЕ ЗАБЫТЬ сделать permissions
		private void FillGameProperties(ClaimsPrincipal user, Game entity, GameEditModel model)
		{
            string currentDescription = entity.Description;

            if (user==null || !user.CanEditGameDescription())
            {
                entity.Description = currentDescription;
            }

            _context.Entry(entity).CurrentValues.SetValues(model);

            if(model.FileImage != null)
            {
                UploadImage(model);
                entity.Imageurl = model.ImgUrl;
            }

			if (entity.Genre == null || entity.Genre.GenreId != model.SelectedGenreId)
			{
				if (model.SelectedGenreId != null)
					entity.Genre = _context.Genres.Find(model.SelectedGenreId);
			}

            if (entity.Rating == null)
            {
                entity.Rating = _context.Ratings.First();
            }

            if (entity.Setting == null)
            {
                entity.Setting = _context.Settings.First();
            }

            entity.Setting = model.Setting;

            entity.Genre.GenreId = model.SelectedGenreId;
            entity.Rating = model.Rating;

			_context.SaveChanges();
		}

		private void UploadImage(GameEditModel model)
		{
			string extension = Path.GetExtension(model.FileImage.FileName);
			string filename = Guid.NewGuid().ToString() + extension;

			model.ImgUrl = filename;

			// путь для сохранения картинки
			string fullFilename = Path.Combine(
				Directory.GetCurrentDirectory(),
				"wwwroot",
				"FileStorage",
				"GameCover",
				filename);

			// сохранить на сервер!
			using (var stream = System.IO.File.Create(fullFilename))
			{
				model.FileImage.CopyTo(stream);
			}
		}

        private void UploadImage(CharachtersEditModel model)
        {
            string extension = Path.GetExtension(model.FileImage.FileName);
            string filename = Guid.NewGuid().ToString() + extension;

            model.ImgUrl = filename;

            // путь для сохранения картинки
            string fullFilename = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "FileStorage",
                "GameCover",
                filename);

            // сохранить на сервер!
            using (var stream = System.IO.File.Create(fullFilename))
            {
                model.FileImage.CopyTo(stream);
            }
        }

        private void UploadImage(WorldEditModel model)
        {
            string extension = Path.GetExtension(model.FileImage.FileName);
            string filename = Guid.NewGuid().ToString() + extension;

            model.ImgUrl = filename;

            // путь для сохранения картинки
            string fullFilename = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "FileStorage",
                "GameCover",
                filename);

            // сохранить на сервер!
            using (var stream = System.IO.File.Create(fullFilename))
            {
                model.FileImage.CopyTo(stream);
            }
        }

        private void PrepareRelations(GameEditModel model, Game entity)
		{
            if (entity.Genre != null)
            {
                model.SelectedGenreId = entity.Genre.GenreId;
            }

            model.GenresList = _context.Genres.Select(b => new SelectListItem()
            {
                Text = b.Name,
                Value = b.GenreId.ToString(),
                Selected = entity.Genre == null ? false : b.GenreId == entity.Genre.GenreId
            }).ToList();


            model.Setting = entity.Setting;
            model.Rating = entity.Rating;
		}

        public List<RoleSimpleModel> GetRoleModels()
        {
			return _roleManager.Roles.Select(r => _mapper.Map<RoleSimpleModel>(r)).ToList();
        }

        public async Task UpdateRoleAsync(RoleEditModel model)
        {

            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
                throw new Exception($"Role [{model.Id}] not found");

            var currentClaims = await _roleManager.GetClaimsAsync(role);
            //1 удаляем права которые не выбраны в модели
            foreach (var item in model.PermissionsList.Where(x => !x.IsChecked))
            {
                var claimToRemove = currentClaims.FirstOrDefault(x => x.Value == item.Name);
                if (claimToRemove != null)
                {
                    await _roleManager.RemoveClaimAsync(role, claimToRemove);
                }
            }

            //2 добавляем права которые выбраны
            foreach (var item in model.PermissionsList.Where(x => x.IsChecked))
            {
                string[] parts = item.Name.Split('.');
                var claimToAdd = currentClaims.FirstOrDefault(x => x.Value == parts[1]);
                if (claimToAdd == null)
                {
                    claimToAdd = new Claim(parts[0], item.Name);
                    await _roleManager.AddClaimAsync(role, claimToAdd);
                }
            }

            //Обновить связи Роли и юзеров
            var currentUsers = await _userManager.GetUsersInRoleAsync(role.Name);

            //1 удаляем юзеров, которые не выбраны в модели
            foreach (var checkBox in model.UsersList.Where(x => !x.IsChecked))
            {
                var user = currentUsers.FirstOrDefault(x => x.UserName == checkBox.Name);
                if (user != null)
                {
                    _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }

            //2 добавляем тех юзеров, которые выбраны
            foreach (var checkBox in model.UsersList.Where(x => x.IsChecked))
            {
                var user = currentUsers.FirstOrDefault(x => x.UserName == checkBox.Name);
                if (user == null)
                {
                    user = await _userManager.FindByNameAsync(checkBox.Name);

                    await _userManager.AddToRoleAsync(user, role.Name);
                }
            }
        }

        public GameEditModel GetGameEditModel(long id)
        {
            var entity = _context.Games
                .Include(g => g.Genre).Include(g => g.Rating).Include(g=>g.Setting).FirstOrDefault(g => g.GameId == id);

            if (entity == null)
                return null;

            var model = _mapper.Map<GameEditModel>(entity);

            PrepareRelations(model, entity);

            return model;
        }

        public GenreEditModel GetGenreEditModel(long id)
        {
            var entity = _context.Genres.FirstOrDefault(g => id == g.GenreId);

            if (entity == null)
                return null;

            var model = _mapper.Map<GenreEditModel>(entity);

            return model;
        }

        public FractionEditModel GetFractionsEditModel(long id)
        {
            var entity = _context.Fractions.FirstOrDefault(g => id == g.FractionsId);

            if (entity == null)
                return null;

            var model = _mapper.Map<FractionEditModel>(entity);

            model.ListOfGames = _context.Games.Select(b => new SelectListItem()
            {
                Text = b.Title,
                Value = b.GameId.ToString(),
                Selected = entity.SettingsOfFractions == null ? false : b.Setting == entity.SettingsOfFractions
            }).ToList();

            return model;
        }

		public WorldEditModel GetWorldEditModel(long id)
		{
			var entity = _context.Worlds.FirstOrDefault(g => id == g.WorldId);

			if (entity == null)
				return null;

			var model = _mapper.Map<WorldEditModel>(entity);

			model.ListOfGames = _context.Games.Select(b => new SelectListItem()
			{
				Text = b.Title,
				Value = b.GameId.ToString(),
				Selected = entity.WorldOfsetting == null ? false : b.Setting == entity.WorldOfsetting
			}).ToList();

			return model;
		}

        public CharachtersEditModel GetCharachtersEditModel(long id)
        {
            var entity = _context.Charachters.FirstOrDefault(g => id == g.CharachtersId);

            if (entity == null)
                return null;

            var model = _mapper.Map<CharachtersEditModel>(entity);

            model.ListOfGames = _context.Games.Select(b => new SelectListItem()
            {
                Text = b.Title,
                Value = b.GameId.ToString(),
                Selected = entity.CharachtersOfSetting == null ? false : b.Setting == entity.CharachtersOfSetting
            }).ToList();

            return model;
        }

        public GameEditModel CreateGameEditModel()
        {
            var entity = new Game();
            var model = new GameEditModel();

            entity.Genre = _context.Genres.First();
            //entity.Rating = _context.Ratings.First();

            PrepareRelations(model, entity);

            return model;
        }

        public void CreateGame(ClaimsPrincipal user, GameEditModel model)
        {
            var entity = new Game();
            var setting = model.Setting = new Setting();
            _context.Games.Add(entity);
            _context.Settings.Add(setting);


            FillGameProperties(user, entity, model);
        }

        public void UpdateGame(ClaimsPrincipal user, GameEditModel model)
        {
            var entity = _context.Games
                .Include(p => p.Genre)
                .Include(p => p.Rating)
                .FirstOrDefault(p => p.GameId == model.GameId);

            if (entity == null)
                throw new Exception("Game not found");

            FillGameProperties(user, entity, model);
        }

        public void DeleteGame(GameEditModel model, Game entity)
        {
            var setting = _context.Settings.Include(f=>f.Fractions).Include(c=>c.Charachters)
                .Include(w=>w.World).FirstOrDefault(s => s.SettingId == model.Setting.SettingId);
            var fractions = _context.Fractions.FirstOrDefault(f => f == setting.Fractions);
            var charachters = _context.Charachters.FirstOrDefault(f => f == setting.Charachters);
            var world = _context.Worlds.FirstOrDefault(f => f == setting.World);
            var rating = _context.Ratings.FirstOrDefault(r=>r == model.Rating);
            
            _context.Remove(setting);
            _context.Remove(fractions);
            _context.Remove(charachters);
            _context.Remove(world);
            _context.Remove(rating);
            _context.Remove(entity);

            _context.SaveChanges();
        }

        public FractionEditModel CreateFractionEditModel()
        {
            var entity = new Fractions();
            var model = new FractionEditModel();

            model.ListOfGames = _context.Games.Select(b => new SelectListItem()
            {
                Text = b.Title,
                Value = b.GameId.ToString(),
                Selected = entity.SettingsOfFractions== null ? false : b.Setting == entity.SettingsOfFractions
            }).ToList();
            
            return model;
        }

        public void CreateFraction(FractionEditModel model)
        {
            //создаём новую фракцию
            var entity = new Fractions();

            //чтобы внести FractionsID в Setting, нужно создать временный FractionsId
            entity.FractionsId = -_context.Fractions.Count();
            _context.Entry(entity).CurrentValues.SetValues(model);

            //здесь связываем 3 таблицы, Game с Setting и Setting с Fractions
            var game = _context.Games.Include(g => g.Setting).FirstOrDefault(g => g.GameId == model.SelectedGameId);
            var setting = _context.Settings.Include(f=>f.Fractions).FirstOrDefault(s => s.SettingId == game.Setting.SettingId);
            setting.Fractions = entity;

            _context.Fractions.Add(entity);
            _context.SaveChanges();
        }

        public WorldEditModel CreateWorldEditModel()
        {
            var model = new WorldEditModel();

            model.ListOfGames = _context.Games.Select(b => new SelectListItem()
            {
                Text = b.Title,
                Value = b.GameId.ToString(),
                Selected = false
            }).ToList();

            return model;
        }

        public void CreateWorld(WorldEditModel model)
        {
            var entity = new World();
            _context.Entry(entity).CurrentValues.SetValues(model);
            if (model.FileImage != null)
            {
                UploadImage(model);
                entity.ImgUrl = model.ImgUrl;
            }

            var game = _context.Games.Include(g => g.Setting)
                .FirstOrDefault(g => g.GameId == model.SelectedGameId);
            if (game == null)
                throw new Exception("Game not found");

            var setting = _context.Settings.Include(s => s.World)
                .FirstOrDefault(s => s.SettingId == game.Setting.SettingId);

            setting.World = entity;

            _context.Worlds.Add(entity);
            _context.SaveChanges();
        }

        public CharachtersEditModel CreateCharachtersEditModel()
        {
            var model = new CharachtersEditModel();

            model.ListOfGames = _context.Games.Select(b => new SelectListItem()
            {
                Text = b.Title,
                Value = b.GameId.ToString(),
                Selected = false
            }).ToList();

            return model;
        }

        public void CreateCharachters(CharachtersEditModel model)
        {
            var entity = new Charachters();
            _context.Entry(entity).CurrentValues.SetValues(model);
            if (model.FileImage != null)
            {
                UploadImage(model);
                entity.ImgUrl = model.ImgUrl;
            }

            var game = _context.Games.Include(g => g.Setting)
                .FirstOrDefault(g => g.GameId == model.SelectedGameId);
            if (game == null)
                throw new Exception("Game not found");

            var setting = _context.Settings.Include(s => s.Charachters)
                .FirstOrDefault(s => s.SettingId == game.Setting.SettingId);

            setting.Charachters = entity;

            _context.Charachters.Add(entity);
            _context.SaveChanges();
        }

        public void DeleteFractions(Fractions fraction)
        {
            var setting = _context.Settings.FirstOrDefault(s => s.Fractions.FractionsId == fraction.FractionsId);
            setting.Fractions = null;
            _context.Remove(fraction);
            _context.SaveChanges();
        }

		public void DeleteWorld(World world)
		{
			var setting = _context.Settings.FirstOrDefault(s => s.World.WorldId== world.WorldId);
			setting.World = null;
			_context.Remove(world);
			_context.SaveChanges();
		}

        public void DeleteCharachters(Charachters charachters)
        {
            var setting = _context.Settings.FirstOrDefault(s => s.Charachters.CharachtersId== charachters.CharachtersId);
            setting.Charachters= null;
            _context.Remove(charachters);
            _context.SaveChanges();
        }
    }
}
