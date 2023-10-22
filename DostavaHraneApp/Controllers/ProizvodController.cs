using DostavaHrane.Data;
using DostavaHrane.Models;
using DostavaHrane.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DostavaHrane.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProizvodController : ControllerBase
    {
        private readonly DostavaHraneContext _context;
        private readonly ILogger<ProizvodController> _logger;
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="context"></param>
        public ProizvodController(DostavaHraneContext context,
            ILogger<ProizvodController> logger)
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
                var proizvod = _context.Proizvod.ToList();

                if (proizvod == null || proizvod.Count == 0)
                {
                    return new EmptyResult();
                }

                List<ProizvodDTO> vrati = new();

                proizvod.ForEach(p =>
                {
                    vrati.Add(new ProizvodDTO()
                    {
                        Sifra = p.Sifra,
                        Naziv = p.Naziv,
                        Opis = p.Opis,
                        Cijena = p.Cijena,
                        Dostupnost = p.Dostupnost,
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
                var p = _context.Dostavljac.Find(sifra);

                if (p == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, p);
                }

                return new JsonResult(p);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }

        }


        [HttpPost]
        public IActionResult Post(ProizvodDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Proizvod p = new Proizvod()
                {
                    Sifra = dto.Sifra,
                    Naziv = dto.Naziv,
                    Opis = dto.Opis,
                    Cijena = dto.Cijena,
                    Dostupnost = dto.Dostupnost,

                };

                _context.Proizvod.Add(p);
                _context.SaveChanges();
                dto.Sifra = p.Sifra;
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
        public IActionResult Put(int sifra, ProizvodDTO pdto)
        {

            if (sifra <= 0 || pdto == null)
            {
                return BadRequest();
            }

            try
            {
                var proizvodBaza = _context.Proizvod.Find(sifra);
                if (proizvodBaza == null)
                {
                    return BadRequest();
                }

                proizvodBaza.Sifra = pdto.Sifra;
                proizvodBaza.Naziv = pdto.Naziv;
                proizvodBaza.Opis = pdto.Opis;
                proizvodBaza.Cijena = pdto.Cijena;
                proizvodBaza.Dostupnost = pdto.Dostupnost;


                _context.Proizvod.Update(proizvodBaza);
                _context.SaveChanges();
                pdto.Sifra = proizvodBaza.Sifra;
                return StatusCode(StatusCodes.Status200OK, pdto);
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
                var Proizvod = _context.Proizvod
                    .Where(p => p.Naziv.Contains(uvjet))

                    // .FromSqlRaw($"SELECT a.* FROM kupac a left join clan b on a.sifra=b.kupac where concat(ime,' ',prezime,' ',ime) like '%@uvjet%'",
                    //             new SqlParameter("uvjet", uvjet), new SqlParameter("kosarica", kosarica))
                    .ToList();
                // (b.kosarica is null or b.kosarica!=@kosarica)  and 
                List<ProizvodDTO> vrati = new();

                Proizvod.ForEach(p => {
                    var pdto = new ProizvodDTO();
                    // dodati u nuget automapper ili neki drugi i skužiti kako se s njim radi, sada ručno
                    vrati.Add(new ProizvodDTO
                    {
                        Sifra = p.Sifra,
                        Naziv = p.Naziv,
                        Opis = p.Opis,
                        Cijena = p.Cijena,
                        Dostupnost = p.Dostupnost,
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

            var proizvodBaza = _context.Proizvod.Find(sifra);
            if (proizvodBaza == null)
            {
                return BadRequest();
            }

            try
            {
                _context.Proizvod.Remove(proizvodBaza);
                _context.SaveChanges();

                return new JsonResult("{\"poruka\":\"Obrisano\"}");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest,
                                  "Ne može se obrisati proizvod koji se nalazi u nekoj kosarici");

            }

        }

    }
}
