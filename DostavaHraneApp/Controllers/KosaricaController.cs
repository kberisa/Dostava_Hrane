using DostavaHrane.Data;
using DostavaHrane.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DostavaHrane.Controllers
{
    [ApiController]
    [Route("api/kosarica")]
    public class KosaricaController : ControllerBase
    {
        private readonly DostavaHraneContext _context;

        public KosaricaController(DostavaHraneContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var kosarice = await _context.Kosarica
                    .Include(k => k.Dostavljac)
                    .Include(k => k.Kupac)
                    .Include(k => k.Proizvod)
                    .ToListAsync();

                return Ok(kosarice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("{sifra:int}")]
        public async Task<IActionResult> Get(int sifra)
        {
            try
            {
                var kosarica = await _context.Kosarica
                    .Include(k => k.Dostavljac)
                    .Include(k => k.Kupac)
                    .Include(k => k.Proizvod)
                    .FirstOrDefaultAsync(k => k.Sifra == sifra);

                if (kosarica == null)
                {
                    return NotFound();
                }

                return Ok(kosarica);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("{sifra:int}")]
        public async Task<IActionResult> Create(Kosarica kosarica)
        {
            try
            {
                _context.Kosarica.Add(kosarica);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetKosarica", new { sifra = kosarica.Sifra }, kosarica);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("{sifra:int}")]
        public async Task<IActionResult> Update(int sifra, Kosarica kosarica)
        {
            if (sifra != kosarica.Sifra)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(kosarica).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KosaricaExists(sifra))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("{sifra:int}")]
        public async Task<IActionResult> Delete(int sifra)
        {
            var kosarica = await _context.Kosarica.FindAsync(sifra);

            if (kosarica == null)
            {
                return NotFound();
            }

            _context.Kosarica.Remove(kosarica);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KosaricaExists(int sifra)
        {
            return _context.Kosarica.Any(k => k.Sifra == sifra);
        }
    }
}
