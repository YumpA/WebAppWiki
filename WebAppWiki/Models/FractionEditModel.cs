using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WebAppWiki.Domains;

namespace WebAppWiki.Models
{
    public class FractionEditModel
    {
        [Display(Name= "ID фракции")]
        public long FractionsId { get; set; }

        [Display(Name = "Название фракции")]
        public string Name { get; set; }

        [Display(Name = "Описание фракции")]
        public string Description { get; set; }

        [AdaptIgnore]
        [ValidateNever]
        public List<SelectListItem> ListOfGames { get; set; }

        [AdaptIgnore]
        public long SelectedGameId { get; set; }

        [AdaptIgnore]
        [HiddenInput(DisplayValue = false)]
        [ValidateNever]
        public string ReturnUrl { get; set; }

        [AdaptIgnore]
        [ValidateNever]
        public List<Setting> SettingsOfFractions { get; set; }
    }
}
