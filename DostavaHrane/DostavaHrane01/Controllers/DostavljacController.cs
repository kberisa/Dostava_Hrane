using DostavaHrane.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DostavaHrane.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DostavljacController : ControllerBase
    {
    }
    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Dohvaćam Dostavljace");

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var dostavljaci = _context.Dostavljac
                .Include(d => d.Dostavljac)
                .Include(k => k.Kosarica)
                .ToList();

            if (dostavljaci == null || dostavljaci.Count == 0)
            {
                return new EmptyResult();
            }

            List<DostavljacDTO> vrati = new();

            dostavljaci.ForEach(d =>
            {
                vrati.Add(new DostavljacDTO()
                {
                    Sifra = d.Sifra,
                    Ime = d.Ime,
                    Smjer = g.Smjer?.Naziv,
                    SifraSmjer = g.Smjer.Sifra,
                    DatumPocetka = g.DatumPocetka,
                    BrojPolaznika = g.Polaznici.Count
                });
            });
            return Ok(vrati);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                ex);
        }


    }
}
