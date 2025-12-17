using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppWiki.DataAccessLayer;
using WebAppWiki.Domains;
using WebAppWiki.Models;

namespace WebAppWiki.BusinessLogic
{
    public class ServiceGame
    {
        private readonly DbContextWiki _context;
        private readonly IMapper _mapper;

        public ServiceGame(DbContextWiki context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public GameDetailsModel GetGameDetailsModel(long id)
        {
            var entity = _context.Games
                .Include(g => g.Genre).Include(g => g.Rating).Include(g => g.Setting)
                .FirstOrDefault(g => g.GameId == id);

            if (entity == null)
                return null;

            var model = _mapper.Map<GameDetailsModel>(entity);

            PrepareRelations(model, entity);

            return model;
        }

        private void PrepareRelations(GameDetailsModel model, Game entity)
        {
            model.Genre=entity.Genre;

            model.Rating = entity.Rating;
        }
    }
}
