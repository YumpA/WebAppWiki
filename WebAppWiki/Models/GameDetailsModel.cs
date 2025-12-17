using Mapster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAppWiki.Domains;

namespace WebAppWiki.Models
{
    public class GameDetailsModel
    {
        [Display(Name = "ID игры")]
        public long GameId { get; set; }

        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Жанр")]
        [AdaptIgnore]
        public Genre? Genre { get; set; }

        [Display(Name = "Рейтинг")]
        [AdaptIgnore]
        public Rating? Rating { get; set; }

        [Display(Name = "Платформа")]
        public string Plot { get; set; }

        [Display(Name = "Url картинки")]
        public string? Imageurl { get; set; }   

        [HiddenInput(DisplayValue = false)]
        [AdaptIgnore]
        public string ReturnUrl { get; set; }

        [Display(Name ="Путь к картинке")]
        public string ImgSrc
        {
            get
            {
                if (String.IsNullOrEmpty(Imageurl))
                {
                    return $"{ModelConstants.GameCover}\\{ModelConstants.ImgNone}";
                }
                else
                {
                    return $"{ModelConstants.GameCover}\\{Imageurl}";
                }
            }
        }
    }
}
