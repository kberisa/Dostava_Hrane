using DostavaHrane.Data;
using DostavaHrane.Models;
using DostavaHrane.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace DostavaHrane.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class KupacController : ControllerBase
    {
    }
    [HttpGet]
    public IActionResult Get()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var kupci = _context.Polaznik.ToList();
        if  kupci == null || kupci.Count == 0)
        {
            return new EmptyResult();
        }

        List<KupacDTO> vrati = new();

        kupci.ForEach(k =>
        {
            // ovo je ručno presipavanje, kasnije upogonimo automapper
            var kdto = new KupacDTO()
            {
                Sifra = k.Sifra,
                KosrisnickoIme = k.KosrisnickoIme,
                Ime = k.Ime,
                Prezime = k.Prezime,
                Telefon = k.Telefon,
                Adresa = k.Adresa
            };

            vrati.Add(kdto);


        });


        return Ok(vrati);

    }



    [HttpGet]
    [Route("{sifra:int}")]
    public IActionResult GetBySifra(int sifra)
    {

        if (sifra <= 0)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var s = _context.Kupac.Find(sifra);

            if (s == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, s);
            }

            return new JsonResult(s);

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
        }

    }




}
