using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppWiki.Abstract;

namespace WebAppWiki.Domains
{
    [Table("Charachters")]
    public class Charachters : MyEntity<long>
    {
        public override long GetId() => CharachtersId;

        [Key]
        public long CharachtersId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string? ImgUrl { get; set; }

        public virtual ICollection<Setting> CharachtersOfSetting { get; set; }

    }
}
