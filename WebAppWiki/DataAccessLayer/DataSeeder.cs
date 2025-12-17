
using Microsoft.AspNetCore.Identity;
using WebAppWiki.Authorize;

namespace WebAppWiki.DataAccessLayer
{
    public class DataSeeder
    {
        private readonly DbContextWiki _context;

        public DataSeeder(DbContextWiki context)
        {
            _context = context;
        }

        public void Seed(IServiceProvider container)
        {
            SeedUserRolesAsync(container).Wait();
        }

        public async Task SeedUserRolesAsync(IServiceProvider container)
        {
            var rolesManager = container.GetRequiredService<RoleManager<IdentityRole>>();

            if(rolesManager.Roles.Count() > 0)
            {
                return;
            }

            string[] roleNames =
            {
                AuthConstants.Roles.Admin,
                AuthConstants.Roles.User
            };

            string[] userNames =
            {
                "yumpa", "vyacheslave"
            };

            foreach (string roleName in roleNames)
            {
                var result = await rolesManager
                    .CreateAsync(new IdentityRole(roleName));
                if (result == null || !result.Succeeded)
                {
                    throw new Exception("Role not created!");
                }
            }
            var userManager = container
                .GetRequiredService<UserManager<AppUser>>();

            for (int i = 0; i < roleNames.Length; i++)
            {
                await AddUserForRoleAsync(userManager, roleNames[i], userNames[i]);
            }
        }

        private async Task AddUserForRoleAsync(UserManager<AppUser> userManager, string roleName, string userName)
        {
            string defaultPassword = "!Qwerty1";
            var user = new AppUser
            {
                UserName = $"{userName}@ya.ru",
                Email = $"{userName}@ya.ru"
            };
            var exists = await userManager.FindByEmailAsync(user.Email);
            IdentityResult result = null;
            if (exists == null)
            {
                result = await userManager.CreateAsync(user, defaultPassword);
                if (result == null || !result.Succeeded)
                {
                    throw new Exception("User not created!");
                }
            }
            result = await userManager.AddToRoleAsync(user, roleName);
            if (result == null || !result.Succeeded)
            {
                throw new Exception("User not Add To Role!");
            }
        }
    }
}
