using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;
using WebAppWiki.Abstract;
using WebAppWiki.DataAccessLayer;
using WebAppWiki.Domains;

namespace WebAppWiki.Helpers
{
    public static class WebBuilderHelper
    {
        public static void DbSeedWithScope(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var container = scope.ServiceProvider;

                var seeder = container.GetService<DataSeeder>();
                seeder?.Seed(container);
            }
        }

        private static void AddRepository<T>(IServiceCollection services)
           where T : MyEntity<long>
        {
            services.AddTransient<IRepository<T, long>, RepositoryGenericSql<T, long>>();
        }

        public static void SetupRepository(this WebApplicationBuilder builder)
        {
            // проверим - какой Репозиторий нужно использовать?
            var section = builder.Configuration
                .GetSection("RepositoryType");

            if (section.Value == "Sql")
            {
                // ищем в конфигурации строку подключения
                var connectionString = builder
                    .Configuration
                    .GetConnectionString("DefaultConnection") ??
                    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

                builder.Services.AddDbContext<DbContextWiki>(
                    options => options.UseSqlServer(connectionString));

                builder.Services.AddScoped<DbContextWiki>();

                // репозиторий MsSQL Database
                AddRepository<Game>(builder.Services);
                AddRepository<Charachters>(builder.Services);
                AddRepository<Fractions>(builder.Services);
                AddRepository<Genre>(builder.Services);
                AddRepository<Rating>(builder.Services);
                AddRepository<Setting>(builder.Services);
                AddRepository<World>(builder.Services);

            }
            else
            {
                throw new Exception("Not supported repository type!");
            }
        }
    }
}
