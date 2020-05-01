using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharma.DbContext;
using Pharma.DbContext.Entities;

namespace Pharma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorToDoctorRatingsController : ControllerBase
    {
        private readonly PharmaContext _context;

        public DoctorToDoctorRatingsController(PharmaContext context)
        {
            _context = context;
        }

        // GET: api/DoctorToDoctorRatings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorToDoctorRating>>> GetDoctorToDoctorRatings()
        {
            return await _context.DoctorToDoctorRatings.ToListAsync();
        }

        // GET: api/DoctorToDoctorRatings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorToDoctorRating>> GetDoctorToDoctorRating(int id)
        {
            var doctorToDoctorRating = await _context.DoctorToDoctorRatings.FindAsync(id);

            if (doctorToDoctorRating == null)
            {
                return NotFound();
            }

            return doctorToDoctorRating;
        }

        // PUT: api/DoctorToDoctorRatings/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctorToDoctorRating(int id, DoctorToDoctorRating doctorToDoctorRating)
        {
            if (id != doctorToDoctorRating.DoctorToDoctorRatingId)
            {
                return BadRequest();
            }

            _context.Entry(doctorToDoctorRating).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorToDoctorRatingExists(id))
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

        // POST: api/DoctorToDoctorRatings
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<DoctorToDoctorRating>> PostDoctorToDoctorRating(DoctorToDoctorRating doctorToDoctorRating)
        {
            _context.DoctorToDoctorRatings.Add(doctorToDoctorRating);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDoctorToDoctorRating", new { id = doctorToDoctorRating.DoctorToDoctorRatingId }, doctorToDoctorRating);
        }

        // DELETE: api/DoctorToDoctorRatings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DoctorToDoctorRating>> DeleteDoctorToDoctorRating(int id)
        {
            var doctorToDoctorRating = await _context.DoctorToDoctorRatings.FindAsync(id);
            if (doctorToDoctorRating == null)
            {
                return NotFound();
            }

            _context.DoctorToDoctorRatings.Remove(doctorToDoctorRating);
            await _context.SaveChangesAsync();

            return doctorToDoctorRating;
        }

        private bool DoctorToDoctorRatingExists(int id)
        {
            return _context.DoctorToDoctorRatings.Any(e => e.DoctorToDoctorRatingId == id);
        }
    }
}
