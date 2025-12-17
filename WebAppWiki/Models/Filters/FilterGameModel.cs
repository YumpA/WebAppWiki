using System.ComponentModel.DataAnnotations;

namespace WebAppWiki.Models.Filters
{
    public class FilterGameModel
    {
        [Display(Name = "Название игры")]
        public string GameName { get; set; }
    }
}
