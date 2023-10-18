using DostavaHrane.Data;
using DostavaHrane.Models;
using DostavaHrane.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DostavaHrane.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DostavljacController : ControllerBase
    {
        private readonly DostavaHraneContext _context;
        private readonly ILogger<DostavljacController> _logger;
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="context"></param>
        public DostavljacController(DostavaHraneContext context,
            ILogger<DostavljacController> logger)
        {
            _context = context;
            _logger = logger;
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
                var dostavljac = _context.Dostavljac();

                if (dostavljac == null || dostavljac.Count = 0)
                {
                    return new EmptyResult();
                }

                List<DostavljacDTO> vrati = new();

                dostavljac.ForEach(d =>
                {
                    vrati.Add(new DostavljacDTO()
                    {
                        Sifra = d.Sifra,
                        Ime = d.Ime,
                        Prezime = d.Prezime,
                        Oib = d.Oib,
                        Email = d.Email,
                        Telefon = d.Telefon
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
        public IActionResult GetBySifra(int sifra)
        {
            if (sifra <= 0)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var d = _context.Dostavljac.Find(sifra);

                if (d == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, d);
                }

                return new JsonResult(d);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }

        }


        [HttpPost]
        public IActionResult Post(DostavljacDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Dostavljac d = new Dostavljac()
                {
                    Sifra = dto.Sifra,
                    Ime = dto.Ime,
                    Prezime = dto.Prezime,
                    Oib = dto.Oib,
                    Email = dto.Email,
                    Telefon = dto.Telefon,
                    
                };

                _context.Dostavljac.Add(d);
                _context.SaveChanges();
                dto.Sifra = d.Sifra;
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
        public IActionResult Put(int sifra, DostavljacDTO ddto)
        {

            if (sifra <= 0 || ddto == null)
            {
                return BadRequest();
            }

            try
            {
                var dostavljacBaza = _context.Dostavljac.Find(sifra);
                if (dostavljacBaza == null)
                {
                    return BadRequest();
                }

                dostavljacBaza.Sifra = ddto.Sifra;
                dostavljacBaza.Ime = ddto.Ime;
                dostavljacBaza.Prezime = ddto.Prezime;
                dostavljacBaza.Oib = ddto.Oib;
                dostavljacBaza.Email = ddto.Email;
                dostavljacBaza.Telefon = ddto.Telefon;
                

                _context.Dostavljac.Update(dostavljacBaza);
                _context.SaveChanges();
                ddto.Sifra = dostavljacBaza.Sifra;
                return StatusCode(StatusCodes.Status200OK, ddto);
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
                var Dostavljac = _context.Dostavljac
                    .Where(d => d.Ime.Contains(uvjet) || d.Prezime.Contains(uvjet))

                    // .FromSqlRaw($"SELECT a.* FROM kupac a left join clan b on a.sifra=b.kupac where concat(ime,' ',prezime,' ',ime) like '%@uvjet%'",
                    //             new SqlParameter("uvjet", uvjet), new SqlParameter("kosarica", kosarica))
                    .ToList();
                // (b.kosarica is null or b.kosarica!=@kosarica)  and 
                List<DostavljacDTO> vrati = new();

                Dostavljac.ForEach(d => {
                    var ddto = new DostavljacDTO();
                    // dodati u nuget automapper ili neki drugi i skužiti kako se s njim radi, sada ručno
                    vrati.Add(new DostavljacDTO
                    {
                        Sifra = d.Sifra,
                        Ime = d.Ime,
                        Prezime = d.Prezime,
                        Oib = d.Oib,
                        Email = d.Email,
                        Telefon = d.Telefon,
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

            var dostavljacBaza = _context.Dostavljac.Find(sifra);
            if (dostavljacBaza == null)
            {
                return BadRequest();
            }

            try
            {
                _context.Dostavljac.Remove(dostavljacBaza);
                _context.SaveChanges();

                return new JsonResult("{\"poruka\":\"Obrisano\"}");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest,
                                  "Ne može se obrisati dostavljac se nalazi na nekoj kosarici");

            }

        }

    }
}
