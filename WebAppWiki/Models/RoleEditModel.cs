using Mapster;
using System.ComponentModel.DataAnnotations;

namespace WebAppWiki.Models
{
    public class RoleEditModel
    {
        [Display(Name = "Id роли")]
        public string Id { get; set; }

        [Display(Name = "Имя роли")]
        public string Name { get; set; }

        [AdaptIgnore]
        public List<CheckBoxItemStringId> PermissionsList { get; set; }

        [AdaptIgnore]
        public List<CheckBoxItemStringId> UsersList { get; set; }
    }
}
