using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace WebAppWiki.Authorize
{
    public class ClaimsHelper
    {
        public static List<IdentityRoleClaim<string>> GetAllPermissions()
        {
            var productEdit = GetPermissions(typeof(AppPermissions.GameEdit));

            return productEdit;
        }

        public static List<IdentityRoleClaim<string>> GetPermissions(Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            var list = fields.Select(x => new IdentityRoleClaim<string>
            {
                ClaimValue = x.Name,
                ClaimType = type.Name
            }).ToList();

            return list;
        }
    }
}
