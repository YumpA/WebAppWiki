using Mapster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAppWiki.Models
{
    public class GenreEditModel
    {
        [Display(Name = "ID жанра")]
        public long GenreId { get; set; }

        [Display(Name = "Название жанра")]   
        public string Name { get; set; }

        [AdaptIgnore]
        [HiddenInput(DisplayValue = false)]
        [ValidateNever]
        public string ReturnUrl { get; set; }
    }
}
