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
	public class DoctorsController : ControllerBase
	{
		private readonly PharmaContext _context;

		public DoctorsController(PharmaContext context)
		{
			_context = context;
		}

		// GET: api/Doctors
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
		{
			return await _context.Doctors.ToListAsync();
		}

		[HttpGet("GetDoctorsByCategory/{category}")]
		public ActionResult<IEnumerable<Doctor>> GetDoctorsByCategory([FromRoute] string category)
		{
			return Ok(_context.Doctors.Where(d => d.Specialization == category).ToList());
		}

		[HttpGet("GetDoctorsByHospitalIdAndCategory/{hospitalId}/{category}")]
		public ActionResult<IEnumerable<Doctor>> GetDoctorsByHospitalIdAndCategory([FromRoute] int hospitalId, [FromRoute] string category)
		{
			return Ok(_context.Doctors.Where(d => d.Specialization == category && _context.DoctorToHospitals.Where(dth => dth.HospitalId == hospitalId)
					.Select(dth => dth.DoctorId)
					.Contains(d.DoctorId)));
		}


		// GET: api/Doctors/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Doctor>> GetDoctor(int id)
		{
			var doctor = await _context.Doctors.FindAsync(id);

			if (doctor == null)
			{
				return NotFound();
			}

			return doctor;
		}

		// PUT: api/Doctors/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutDoctor(int id, Doctor doctor)
		{
			if (id != doctor.DoctorId)
			{
				return BadRequest();
			}

			_context.Entry(doctor).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!DoctorExists(id))
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

		// POST: api/Doctors
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPost]
		public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)
		{
			_context.Doctors.Add(doctor);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetDoctor", new { id = doctor.DoctorId }, doctor);
		}

		// DELETE: api/Doctors/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<Doctor>> DeleteDoctor(int id)
		{
			var doctor = await _context.Doctors.FindAsync(id);
			if (doctor == null)
			{
				return NotFound();
			}

			_context.Doctors.Remove(doctor);
			await _context.SaveChangesAsync();

			return doctor;
		}

		private bool DoctorExists(int id)
		{
			return _context.Doctors.Any(e => e.DoctorId == id);
		}
	}
}
