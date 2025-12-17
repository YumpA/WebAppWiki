using WebAppWiki.BusinessLogic.Filters;
using WebAppWiki.DataAccessLayer;
using WebAppWiki.Domains;
using WebAppWiki.Helpers;
using WebAppWiki.Models.Filters;

namespace WebAppWiki.BusinessLogic
{
    public class ServiceFilter
    {
        private readonly ISession? _session;
        private readonly DbContextWiki _context;

        public ServiceFilter(DbContextWiki context, IHttpContextAccessor accessor)
        {
            _session = accessor?.HttpContext?.Session;
            _context = context;
            
        }

        public FilterMainModel ProjectToModel(FilterMain filter)
        {
            var model = new FilterMainModel();
            model.FilterGame = GetFilterGameModel(filter);

            return model;
        }

        public void FiltersUpdate(FilterMain filter, FilterMainModel model)
        {
            if(model.FilterGame != null)
            {
                if (filter.FilterGame == null)
                    filter.FilterGame = new FilterGame();

                filter.FilterGame.GameName = model.FilterGame.GameName;
                filter.FilterGame.ListGameId = null;
            }
        }

        private FilterGameModel GetFilterGameModel(FilterMain filter)
        {
            var modelGame = new FilterGameModel();

            modelGame.GameName=filter.FilterGame?.GameName ?? string.Empty;

            return modelGame;
        }

        public IQueryable<Game> ApplyFilter(FilterMain filterMain)
        {
            IQueryable<Game> query = _context.Games.AsQueryable();

            if (filterMain == null)
                return query;

            if(filterMain.FilterGame != null)
                query = ApplyGame(filterMain.FilterGame, query);

            _session?.SaveObject(filterMain);

            return query;
        }

        private IQueryable<Game> ApplyGame(FilterGame filterGame, IQueryable<Game> query)
        {
            var listGameId = filterGame.ListGameId;
            var listGenresId = filterGame.ListGenresId;

            if(listGameId == null || listGameId.Count == 0)
            {
                if (!string.IsNullOrEmpty(filterGame.GameName))
                {
                    string gameName = filterGame.GameName.ToLower();
                    listGameId = _context.Games
                        .Where(g => g.Title.ToLower().Contains(gameName)).Select(g => g.GameId).ToList();
                    listGenresId = _context.Games
                        .Where(g => g.Title.ToLower().Contains(gameName)).Select(g => g.Genre.GenreId).ToList();

                    filterGame.ListGameId = listGameId;
                    filterGame.ListGenresId = listGenresId;

                }
            }

            if (listGameId != null && listGameId.Count > 0)
            {
                query = query.Where(g => g.Title == filterGame.GameName );
            }

            return query;
        }
    }
}
