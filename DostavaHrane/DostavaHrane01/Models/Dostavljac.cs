using System.ComponentModel.DataAnnotations.Schema;

namespace DostavaHrane.Models
{
	public class Dostavljac : Entitet
	{
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public string? Oib { get; set; }
        public string? Email { get; set; }
        public string? Telefon { get; set; }
    }
}