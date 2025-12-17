using System.Security.Claims;

namespace WebAppWiki.Authorize
{
    public static class PermissionHelper
    {
        public static bool CanEditGameDescription(this ClaimsPrincipal user)
        {
            return user.Claims.Any(x => x.Value == AppPermissions.GameEdit.Description);
        }
    }
}
