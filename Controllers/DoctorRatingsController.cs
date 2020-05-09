using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Pharma.DbContext;
using Pharma.DbContext.Entities;

namespace Pharma.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DoctorRatingsController : ControllerBase
	{
		private readonly PharmaContext _context;

		public DoctorRatingsController(PharmaContext context)
		{
			_context = context;
		}

		// GET: api/DoctorRatings
		[HttpGet]
		public async Task<ActionResult<IEnumerable<DoctorRating>>> GetDoctorRatings()
		{
			return await _context.DoctorRatings.ToListAsync();
		}

		[HttpGet("GetDoctorRatingsByCategory/{category}")]
		public ActionResult<IEnumerable<DoctorToDoctorRating>> GetDoctorRatingsByCategory(string category)
		{
			var result =
				from doctorToDoctorRatings in _context.DoctorToDoctorRatings
				join doctors in _context.Doctors on doctorToDoctorRatings.DoctorId equals doctors.DoctorId
				join doctorRatings in _context.DoctorRatings on doctorToDoctorRatings.DoctorRatingId equals
					doctorRatings.DoctorRatingId
				where doctors.Specialization == category && doctorRatings.Name == category
				orderby doctorToDoctorRatings.RankingPlace
				select new DoctorToDoctorRating
				{
					DoctorToDoctorRatingId = doctorToDoctorRatings.DoctorToDoctorRatingId,
					DoctorId = doctorToDoctorRatings.DoctorId,
					DoctorRatingId = doctorToDoctorRatings.DoctorRatingId,
					RankingPlace = doctorToDoctorRatings.RankingPlace
				};

			var resultList = result.ToList();

			for (var i = 0; i < resultList.Count; i++)
			{
				resultList[i].RankingPlace = i + 1;
			}

			return Ok(resultList);
		}

		// GET: api/DoctorRatings/5
		[HttpGet("{id}")]
		public async Task<ActionResult<DoctorRating>> GetDoctorRating(int id)
		{
			var doctorRating = await _context.DoctorRatings.FindAsync(id);

			if (doctorRating == null)
			{
				return NotFound();
			}

			return doctorRating;
		}

		// PUT: api/DoctorRatings/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see https://aka.ms/RazorPagesCRUD.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutDoctorRating(int id, DoctorRating doctorRating)
		{
			if (id != doctorRating.DoctorRatingId)
			{
				return BadRequest();
			}

			_context.Entry(doctorRating).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!DoctorRatingExists(id))
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

		// POST: api/DoctorRatings
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see https://aka.ms/RazorPagesCRUD.
		[HttpPost]
		public async Task<ActionResult<DoctorRating>> PostDoctorRating(DoctorRating doctorRating)
		{
			_context.DoctorRatings.Add(doctorRating);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetDoctorRating", new { id = doctorRating.DoctorRatingId }, doctorRating);
		}

		// DELETE: api/DoctorRatings/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<DoctorRating>> DeleteDoctorRating(int id)
		{
			var doctorRating = await _context.DoctorRatings.FindAsync(id);
			if (doctorRating == null)
			{
				return NotFound();
			}

			_context.DoctorRatings.Remove(doctorRating);
			await _context.SaveChangesAsync();

			return doctorRating;
		}

		private bool DoctorRatingExists(int id)
		{
			return _context.DoctorRatings.Any(e => e.DoctorRatingId == id);
		}
	}
}
