using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppWiki.Abstract;

namespace WebAppWiki.Domains
{
    [Table("Genre")]
    public class Genre: MyEntity<long>
    {
        public override long GetId() => GenreId;

        [Key]
        public long GenreId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Game> GenreOfGames { get; set; }
    }
}
