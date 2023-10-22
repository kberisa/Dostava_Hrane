using DostavaHrane.Data;
using DostavaHrane.Models;
using DostavaHrane.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DostavaHrane.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class KupacController : ControllerBase
    {
        private readonly DostavaHraneContext _context;
        private readonly ILogger<KupacController> _logger;

        public KupacController(DostavaHraneContext context,
            ILogger<KupacController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Dohvaćam Kupce");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var kupci = _context.Kupac.ToList();
            if (kupci == null || kupci.Count == 0)
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
                    KorisnickoIme = k.KorisnickoIme,
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
                var k = _context.Kupac.Find(sifra);

                if (k == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, k);
                }

                return new JsonResult(k);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }

        }


        [HttpPost]
        public IActionResult Post(KupacDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Kupac k = new Kupac()
                {
                    KorisnickoIme = dto.KorisnickoIme,
                    Ime = dto.Ime,
                    Prezime = dto.Prezime,
                    Telefon = dto.Telefon,
                    Adresa = dto.Adresa
                };

                _context.Kupac.Add(k);
                _context.SaveChanges();
                dto.Sifra = k.Sifra;
                return Ok(dto);

            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }


        [HttpPut]
        [Route("{sifra:int}")]
        public IActionResult Put(int sifra, KupacDTO kdto)
        {

            if (sifra <= 0 || kdto == null)
            {
                return BadRequest();
            }

            try
            {
                var kupacBaza = _context.Kupac.Find(sifra);
                if (kupacBaza == null)
                {
                    return BadRequest();
                }

                kupacBaza.KorisnickoIme = kdto.KorisnickoIme;
                kupacBaza.Ime = kdto.Ime;
                kupacBaza.Prezime = kdto.Prezime;
                kupacBaza.Telefon = kdto.Telefon;
                kupacBaza.Adresa = kdto.Adresa;

                _context.Kupac.Update(kupacBaza);
                _context.SaveChanges();
                kdto.Sifra = kupacBaza.Sifra;
                return StatusCode(StatusCodes.Status200OK, kdto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                                  ex);
            }
        }


        [HttpGet]
        [Route("trazi/{uvjet}")]
        public IActionResult TraziKupac(string uvjet)
        {

            if (uvjet == null || uvjet.Length < 3)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var kupci = _context.Kupac
                    .Include(k => k.Dostavljaci)
                    .Include(k => k.Proizvodi)
                    .Where(k => k.Ime.Contains(uvjet) || k.Prezime.Contains(uvjet))

                    // .FromSqlRaw($"SELECT a.* FROM kupac a left join clan b on a.sifra=b.kupac where concat(ime,' ',prezime,' ',ime) like '%@uvjet%'",
                    //             new SqlParameter("uvjet", uvjet), new SqlParameter("kosarica", kosarica))
                    .ToList();
                // (b.kosarica is null or b.kosarica!=@kosarica)  and 
                List<KupacDTO> vrati = new();

                kupci.ForEach(k => {
                    var kdto = new KupacDTO();
                    // dodati u nuget automapper ili neki drugi i skužiti kako se s njim radi, sada ručno
                    vrati.Add(new KupacDTO
                    {
                        Sifra = k.Sifra,
                        KorisnickoIme = k.KorisnickoIme,
                        Ime = k.Ime,
                        Prezime = k.Prezime,
                        Telefon = k.Telefon,
                        Adresa = k.Adresa
                    });
                });


                return new JsonResult(vrati);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
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

            var kupacBaza = _context.Kupac.Find(sifra);
            if (kupacBaza == null)
            {
                return BadRequest();
            }

            try
            {
                _context.Kupac.Remove(kupacBaza);
                _context.SaveChanges();

                return new JsonResult("{\"poruka\":\"Obrisano\"}");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest,
                                  "Ne može se obrisati kupac koji se nalazi u nekoj kosarici");

            }

        }

    }
}
