using Mapster;
using WebAppWiki.Domains;
using WebAppWiki.Models;

namespace WebAppWiki.Mapping
{
    public class RegisterMapper
    {
        public static void RegisterSettings()
        {
            TypeAdapterConfig.GlobalSettings
                .NewConfig<Game, GameSimpleModel>()
                .Map(dst => dst.Id, src => src.GameId)
                .RequireDestinationMemberSource(true);

            TypeAdapterConfig.GlobalSettings
                .NewConfig<Game, GameDetailsModel>()
                .RequireDestinationMemberSource(true);

            TypeAdapterConfig.GlobalSettings
                .NewConfig<Game, GameEditModel>()
                .RequireDestinationMemberSource(true);

            TypeAdapterConfig.GlobalSettings
                .NewConfig<Genre, GenreEditModel>()
                .RequireDestinationMemberSource(true);
        }
    }
}
