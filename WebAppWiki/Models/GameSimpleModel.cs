using System.ComponentModel.DataAnnotations;

namespace WebAppWiki.Models
{
    public class GameSimpleModel
    {
        public long Id { get; set; }

        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Платформа")]
        public string Plot { get; set; }
    }
}
