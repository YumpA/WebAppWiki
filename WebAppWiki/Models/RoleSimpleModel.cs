using System.ComponentModel.DataAnnotations;

namespace WebAppWiki.Models
{
    public class RoleSimpleModel
    {
        [Display(Name = "Id роли")]
        public string Id { get; set; }

        [Display(Name = "Имя роли")]
        public string Name { get; set; }
    }
}
