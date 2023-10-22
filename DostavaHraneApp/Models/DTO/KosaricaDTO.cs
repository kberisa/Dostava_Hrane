using System.ComponentModel.DataAnnotations.Schema;

namespace DostavaHrane.Models.DTO
{
    public class KosaricaDTO
    {
        public int Sifra { get; set; }
        public string? Proizvod { get; set; }
        public int Kolicina { get; set; }
        public string? Kupac { get; set; }
        public string? AdresaDostave { get; set; }
        public string? StatusDostave { get; set; }
        public string? Dostavljac { get; set; }
        public object? SifraKupac { get; set; }
    }
}
