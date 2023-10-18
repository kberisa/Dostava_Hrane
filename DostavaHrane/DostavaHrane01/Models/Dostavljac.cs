using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace DostavaHrane.Models
{
    public class Dostavljac : Entitet
    {
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public string? Oib { get; set; }
        public string? Email { get; set; }
        public string? Telefon { get; set; }

        public ICollection<Proizvod> Proizvodi { get; } = new List<Proizvod>();
        public ICollection<Kupac> Kupci { get; } = new List<Kupac>();
    }
}