using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppWiki.Abstract;

namespace WebAppWiki.Domains
{
    [Table("Setting")]
    public class Setting : MyEntity<long>
    {
        public override long GetId() => SettingId;

        [Key]
        public long SettingId { get; set; }
        
        public Charachters? Charachters { get; set; }
        
        public Fractions? Fractions { get; set; }
        
        public World? World { get; set; }

        public virtual ICollection<Game> SettingOfGames { get; set; }
    }
}
