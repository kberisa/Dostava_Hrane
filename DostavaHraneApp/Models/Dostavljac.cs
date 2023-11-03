using DostavaHrane.Models;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DostavaHrane.Models
{
    public class Dostavljac : Entitet
    {
        
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public string? Oib { get; set; }
        public string? Email { get; set; }
        public string? Telefon { get; set; }

        public ICollection<Proizvod> DostavljacProizvodi { get; } = new List<Proizvod>();
        public ICollection<Kupac> DostavljacKupci { get; } = new List<Kupac>();
        public ICollection<Kosarica> DostavljacKosarice { get; } = new List<Kosarica>();
        public object KosaricaDostavljaci { get; internal set; }
    }
}