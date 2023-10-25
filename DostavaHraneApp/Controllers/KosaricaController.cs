using DostavaHrane.Data;
using DostavaHrane.Models;
using DostavaHrane.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DostavaHrane.Mappings;
using AutoMapper;

namespace DostavaHrane.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class KosaricaController : ControllerBase
    {
        private readonly DostavaHraneContext _context;
        private readonly ILogger<KosaricaController> _logger;
        private readonly IMapper _mapper;
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="context"></param>
        public KosaricaController(DostavaHraneContext context,
            ILogger<KosaricaController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
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
                return Ok(kosarice.MapKosarica());
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

            var kosarica = _context.Kosarica
                .Include(k => k.Kupac)
                .FirstOrDefault(k => k.Sifra == sifra);

            if (kosarica == null)
            {
                return new EmptyResult();
            }

            var kosaricaDTO = _mapper.Map<KosaricaDTO>(kosarica);

            return Ok(kosaricaDTO);
        }

        [HttpPost]
        public IActionResult Post(KosaricaDTO kosaricaDTO)
        {
            if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

            if (string.IsNullOrEmpty(kosaricaDTO.Proizvod))
                {
                    return BadRequest(ModelState);
                }

            try
                {

                var kosarica = _mapper.Map<Kosarica>(kosaricaDTO);
                var proizvod = _context.Proizvod.Find(kosaricaDTO.Proizvod);

            if (proizvod == null)
                 {
                    return BadRequest(ModelState);
                 }

                    kosarica.Proizvod = proizvod;

                    _context.Kosarica.Add(kosarica);
                    _context.SaveChanges();

                    var KosaricaDTO = _mapper.Map<KosaricaDTO>(kosarica);

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
            if (!ModelState.IsValid || sifra <= 0 || kosaricaDTO == null)
            {
                return BadRequest();
            }

            try
            {   
                var kosarica = _context.Kosarica.Find(sifra);

                if (kosarica == null)
                {
                    return BadRequest();
                }
 
                _mapper.Map(kosaricaDTO, kosarica);
         
                _context.Kosarica.Update(kosarica);
                _context.SaveChanges();

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
            if (!ModelState.IsValid || sifra <= 0)
            {
                return BadRequest();
            }

            try
            {
                var kosarica = _context.Kosarica
                    .Include(k => k.Proizvodi)
                    .FirstOrDefault(k => k.Sifra == sifra);

                if (kosarica == null || kosarica.Proizvodi == null || kosarica.Proizvodi.Count == 0)
                {
                    return new EmptyResult();
                }

                // Use AutoMapper to map Proizvod entities to ProizvodDTO.
                var proizvodiDTO = _mapper.Map<List<ProizvodDTO>>(kosarica.Proizvodi);

                return Ok(proizvodiDTO);
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
            [Route("{sifra:int}/dodaj/{proizvodiSifra:int}")]
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