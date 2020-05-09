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
	public class HospitalsController : ControllerBase
	{
		private readonly PharmaContext _context;

		public HospitalsController(PharmaContext context)
		{
			_context = context;
		}

		// GET: api/Hospitals
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Hospital>>> GetHospitals()
		{
			return await _context.Hospitals.ToListAsync();
		}

		[HttpGet("GetHospitalsByDoctorsCategory/{category}")]
		public ActionResult<IEnumerable<Hospital>> GetHospitalsByDoctorsCategory(string category)
		{
			var hospitalsByDoctorsCategory = _context.Hospitals
				.Where(h => _context.DoctorToHospitals
					.Where(dth => _context.Doctors
								.Where(d => d.Specialization == category)
								.Select(d => d.DoctorId)
								.Contains(dth.DoctorId))
					.Select(dth => dth.HospitalId)
					.Contains(h.HospitalId))
				.ToList();

			return Ok(hospitalsByDoctorsCategory);
		}

		// GET: api/Hospitals/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Hospital>> GetHospital(int id)
		{
			var hospital = await _context.Hospitals.FindAsync(id);

			if (hospital == null)
			{
				return NotFound();
			}

			return hospital;
		}

		// PUT: api/Hospitals/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutHospital(int id, Hospital hospital)
		{
			if (id != hospital.HospitalId)
			{
				return BadRequest();
			}

			_context.Entry(hospital).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!HospitalExists(id))
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

		// POST: api/Hospitals
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPost]
		public async Task<ActionResult<Hospital>> PostHospital(Hospital hospital)
		{
			_context.Hospitals.Add(hospital);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetHospital", new { id = hospital.HospitalId }, hospital);
		}

		// DELETE: api/Hospitals/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<Hospital>> DeleteHospital(int id)
		{
			var hospital = await _context.Hospitals.FindAsync(id);
			if (hospital == null)
			{
				return NotFound();
			}

			_context.Hospitals.Remove(hospital);
			await _context.SaveChangesAsync();

			return hospital;
		}

		private bool HospitalExists(int id)
		{
			return _context.Hospitals.Any(e => e.HospitalId == id);
		}
	}
}
