using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppWiki.Abstract;

namespace WebAppWiki.Domains
{
    [Table("World")]
    public class World : MyEntity<long>
    {
        public override long GetId() => WorldId;

        [Key]
        public long WorldId { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public string? ImgUrl { get; set; }

        public virtual ICollection<Setting> WorldOfsetting { get; set; }
    }
}
