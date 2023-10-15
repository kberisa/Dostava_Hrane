using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DostavaHrane.Models
{
	public class Kupac : Entitet
	{
        public int Sifra { get; set; }
        public string? KorisnickoIme { get; set; }
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public string? Adresa { get; set; }
        public string? Telefon { get; set; }


    }
}
