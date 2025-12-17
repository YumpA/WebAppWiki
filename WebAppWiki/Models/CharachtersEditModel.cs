using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WebAppWiki.Domains;

namespace WebAppWiki.Models
{
    public class CharachtersEditModel
    {
        [Display(Name = "ID персонажа")]
        public long CharachtersId { get; set; }
        
        [Display(Name = "Имя персонажа")]
        public string Name { get; set; }

        [Display(Name = "Описание персонажа")]
        public string Description { get; set; }

        [Display(Name = "Url картинки")]
        [AdaptIgnore]
        public string? ImgUrl { get; set; }

        [AdaptIgnore]
        [ValidateNever]
        public List<SelectListItem> ListOfGames { get; set; }

        [Display(Name = "Фото персонажа")]
        [AdaptIgnore]
        public IFormFile? FileImage { get; set; }

        [AdaptIgnore]
        public long SelectedGameId { get; set; }

        [AdaptIgnore]
        [HiddenInput(DisplayValue = false)]
        [ValidateNever]
        public string ReturnUrl { get; set; }

        public string ImgSrc
		{
			get
			{
				if (String.IsNullOrEmpty(ImgUrl))
				{
					return $"{ModelConstants.GameCover}\\{ModelConstants.ImgNone}";
				}
				else
				{
					return $"{ModelConstants.GameCover}\\{ImgUrl}";
				}
			}
		}
	}
}


