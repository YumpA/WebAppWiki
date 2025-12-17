using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppWiki.Abstract;

namespace WebAppWiki.Domains
{
    [Table("Rating")]
    public class Rating : MyEntity<long>
    {
        public override long GetId() => RatingId;

        [Key]
        public long RatingId { get; set; }

        public int Score { get; set; }

        public virtual ICollection<Game>? GamesOfRating { get; set; }
    }
}
