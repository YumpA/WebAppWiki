using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppWiki.Abstract;

namespace WebAppWiki.Domains
{
    [Table("Fractions")]
    public class Fractions : MyEntity<long>
    {
        public override long GetId() => FractionsId;

        [Key]
        public long FractionsId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Setting> SettingsOfFractions { get; set; }
    }
}
