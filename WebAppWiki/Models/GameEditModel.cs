using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppWiki.Domains;

namespace WebAppWiki.Models
{
	public class GameEditModel
	{
		[Display(Name = "ID игры")]
		public long GameId { get; set; }

		[Display(Name = "Название")]
		public string Title { get; set; }

		[Display(Name = "Описание")]
		public string Description { get; set; }

		[Display(Name = "Платформа")]
		public string Plot { get; set; }

		[Display(Name = "Url картинки")]
		[AdaptIgnore]
		public string? ImgUrl { get; set; }

		[Display(Name ="Обложка игры")]
		[AdaptIgnore]
        public IFormFile? FileImage { get; set; }

        [AdaptIgnore]
        [ValidateNever]
		[HiddenInput]
		public Setting Setting { get; set; }

		[AdaptIgnore]
		[ValidateNever]
		public List<SelectListItem> GenresList { get; set; }

		[AdaptIgnore]
        public long SelectedGenreId { get; set; }

        [Display(Name = "Рейтинг игры")]
        [AdaptIgnore]
        public Rating Rating{ get; set; }

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
