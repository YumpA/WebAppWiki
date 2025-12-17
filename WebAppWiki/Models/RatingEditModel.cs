using Mapster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using WebAppWiki.Domains;

namespace WebAppWiki.Models
{
    public class RatingEditModel
    {
        public long RatingId { get; set; }

        public int Score { get; set; }

        [AdaptIgnore]
        [ValidateNever]
        public List<Game>? GamesOfRating { get; set; }
    }
}
