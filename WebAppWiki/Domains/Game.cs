using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppWiki.Abstract;

namespace WebAppWiki.Domains
{
    [Table("Game")]
    public class Game : MyEntity<long>
    {
        public override long GetId() => GameId;

        [Key]
        public long GameId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Plot { get; set; }

        public virtual Rating? Rating { get; set; }

        public virtual Setting? Setting { get; set; }

        public virtual Genre? Genre { get; set; }

        public string? Imageurl { get; set; }
    }
}
