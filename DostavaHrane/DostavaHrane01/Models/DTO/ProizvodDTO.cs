

namespace DostavaHrane.Models.DTO
{
    public class ProizvodDTO
    {
        public int Sifra { get; set; }
        public string? Naziv { get; set; }
        public string? Opis { get; set; }
        public decimal? Cijena { get; set; }
        public bool? Dostupnost { get; set; }
    }
}
