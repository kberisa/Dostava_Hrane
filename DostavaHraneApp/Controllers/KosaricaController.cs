using DostavaHrane.Data;
using DostavaHrane.Models;
using DostavaHrane.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DostavaHrane.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class KosaricaController : ControllerBase
    {
        private readonly DostavaHraneContext _context;
        private readonly ILogger<KosaricaController> _logger;
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="context"></param>
        public KosaricaController(DostavaHraneContext context,
            ILogger<KosaricaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Dohvaćam kosaricu");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var kosarice = _context.Kosarica
                    .Include(k => k.Kupac)
                    .Include(k => k.Proizvod)
                    .Include(k => k.Dostavljac)
                    .ToList();

                if (kosarice == null || kosarice.Count == 0)
                {
                    return new EmptyResult();
                }

                List<KosaricaDTO> vrati = new();

                kosarice.ForEach(k =>
                {
                    vrati.Add(new KosaricaDTO()
                    {
                        Sifra = k.Sifra,
                        Proizvod = k.Proizvod,
                        Kolicina = k.Kolicina,
                        Kupac = k.Kupac,
                        AdresaDostave = k.AdresaDostave,
                        StatusDostave = k.StatusDostave,
                        Dostavljac = k.Dostavljac,
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

        [HttpGet]
        [Route("{sifra:int}")]
        public IActionResult GetById(int sifra)
        {
           

            if (sifra == 0)
            {
                return BadRequest(ModelState);
            }

            var k = _context.Kosarica.Include(k => k.Kupac)
              .FirstOrDefault(k => k.Sifra == sifra);

            if (k == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, k); 
            }

            try
            {
                return new JsonResult(new KosaricaDTO()
                {
                    Sifra = k.Sifra,
                    Proizvod = k.Proizvod,
                    Kolicina = k.Kolicina,
                    Kupac = k.Kupac,
                    AdresaDostave = k.AdresaDostave,
                    StatusDostave = k.StatusDostave,
                    Dostavljac = k.Dostavljac                       
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }



        [HttpPost]
        public IActionResult Post(KosaricaDTO kosaricaDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (KosaricaDTO.Proizvod == 0)
            {
                return BadRequest(ModelState);
            }

            try
            {
  
                var proizvod = _context.Proizvod.Find(KosaricaDTO.Proizvod);
                

                if (proizvod = null)
                {
                    return BadRequest(ModelState);
                }
                
                    Kosarica k = new()
                {
                    Proizvod = kosaricaDTO.Proizvod,
                    Kolicina = kosaricaDTO.Kolicina,
                    AdresaDostave = kosaricaDTO.AdresaDostave
                };

                _context.Kosarica.Add(k);
                _context.SaveChanges();

                    KosaricaDTO.Sifra = k.Sifra;
                    KosaricaDTO.Proizvod = k.Proizvod;
                    KosaricaDTO.Kolicina = k.Kolicina;
                    KosaricaDTO.Kupac = k.Kupac;
                    KosaricaDTO.AdresaDostave = k.AdresaDostave;
                    KosaricaDTO.StatusDostave = k.StatusDostave;
                    KosaricaDTO.Dostavljac = k.Dostavljac;

                return Ok(KosaricaDTO);


            }
            catch (Exception ex)
            {
                return StatusCode(
                   StatusCodes.Status503ServiceUnavailable,
                   ex);
            }

        }


        [HttpPut]
        [Route("{sifra:int}")]
        public IActionResult Put(int sifra, KosaricaDTO kosaricaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (sifra <= 0 || kosaricaDTO == null)
            {
                return BadRequest();
            }

            try
            {
                var kosarica = _context.sifra.Find(kosaricaDTO.Sifra);

                if (kosarica == null)
                {
                    return BadRequest();
                }

                var kosarica = _context.Kosarica.Find(sifra);

                if (kosarica == null)
                {
                    return BadRequest();
                }

                kosarica.Kosarica = kosaricaDTO;

                _context.Kosarica.Update(kosarica);
                _context.SaveChanges();

                kosaricaDTO.Sifra = sifra;
                kosaricaDTO.Kupac = kupac.Ime;

                return Ok(kosaricaDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    ex.Message);
            }


        }



        [HttpDelete]
        [Route("{sifra:int}")]
        [Produces("application/json")]
        public IActionResult Delete(int sifra)
        {
            if (sifra <= 0)
            {
                return BadRequest();
            }

            var kosaricaBaza = _context.Kosarica.Find(sifra);
            if (kosaricaBaza == null)
            {
                return BadRequest();
            }

            try
            {
                _context.Kosarica.Remove(kosaricaBaza);
                _context.SaveChanges();

                return new JsonResult("{\"poruka\":\"Obrisano\"}");

            }
            catch (Exception ex)
            {

                return new JsonResult("{\"poruka\":\"Ne može se obrisati\"}");

            }
        }



        [HttpGet]
        [Route("{sifra:int}/Proizvodi")]
        public IActionResult GetProizvodi(int sifra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (sifra <= 0)
            {
                return BadRequest();
            }

            try
            {
                var kosarica = _context.Kosarica
                    .Include(k => k.Proizvodi)
                    .FirstOrDefault(k => k.Sifra == sifra);

                if (kosarica == null)
                {
                    return BadRequest();
                }

                if (kosarica.Proizvodi == null || kosarica.Proizvodi.Count == 0)
                {
                    return new EmptyResult();
                }

                List<ProizvodDTO> vrati = new();
                kosarica.Proizvodi.ForEach(k =>
                {
                    vrati.Add(new ProizvodDTO()
                    {
                        Sifra = k.Sifra,
                        Proizvod = k.Proizvod,
                        Kolicina = k.Kolicina,
                        Kupac = k.Kupac,
                        AdresaDostave = k.AdresaDostave,
                        StatusDostave = k.StatusDostave,
                        Dostavljac = k.Dostavljac
                    });
                });
                return Ok(vrati);
            }
            catch (Exception ex)
            {
                return StatusCode(
                        StatusCodes.Status503ServiceUnavailable,
                        ex.Message);
            }


        }

        [HttpPost]
        [Route("{sifra:int}/dodaj/{ProizvodiSifra:int}")]
        public IActionResult DodajProizvodi(int sifra, int proizvodSifra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (sifra <= 0 || proizvodSifra <= 0)
            {
                return BadRequest();
            }

            try
            {

                var kosarica = _context.Kosarica
                    .Include(k => k.Proizvod)
                    .FirstOrDefault(k => k.Sifra == sifra);

                if (kosarica == null)
                {
                    return BadRequest();
                }

                var proizvod = _context.Proizvod.Find(proizvodSifra);

                if (proizvodSifra == null)
                {
                    return BadRequest();
                }

                // napraviti kontrolu da li je taj proizvod već u toj kosarici
                kosarica.Proizvodi.Add(proizvod);

                _context.Kosarica.Update(kosarica);
                _context.SaveChanges();

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(
                       StatusCodes.Status503ServiceUnavailable,
                       ex.Message);

            }

        }

        [HttpDelete]
        [Route("{sifra:int}/dodaj/{proizvodSifra:int}")]
        public IActionResult ObrisiProizvod(int sifra, int proizvodSifra)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (sifra <= 0 || proizvodSifra <= 0)
            {
                return BadRequest();
            }

            try
            {

                var kosarica = _context.Kosarica
                    .Include(k => k.Proizvod)
                    .FirstOrDefault(k => k.Sifra == sifra);

                if (kosarica == null)
                {
                    return BadRequest();
                }

                var proizvod = _context.Proizvod.Find(proizvodSifra);

                if (proizvod == null)
                {
                    return BadRequest();
                }


                kosarica.Proizvodi.Remove(proizvod);

                _context.Kosarica.Update(kosarica);
                _context.SaveChanges();

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(
                       StatusCodes.Status503ServiceUnavailable,
                       ex.Message);

            }

        }
    }
}
