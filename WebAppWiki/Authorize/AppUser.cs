using Microsoft.AspNetCore.Identity;

namespace WebAppWiki.Authorize
{
    public class AppUser : IdentityUser
    {
        public string? Firstame { get; set; }

        public string? Lastname { get; set; }
    }
}
