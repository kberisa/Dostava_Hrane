using System.ComponentModel.DataAnnotations;

namespace DostavaHrane.Models
{
    public abstract class Entitet
    {
        [Key]
        public int Sifra { get; set; }


    }
}
