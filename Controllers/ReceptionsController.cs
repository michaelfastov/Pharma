using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharma.DbContext;
using Pharma.DbContext.Entities;
using Pharma.Helpers;
using Pharma.ViewModels;

namespace Pharma.Controllers
{
	[Authorize(AuthenticationSchemes = "Bearer", Policy = "ApiUser")]
	[Route("api/[controller]")]
	[ApiController]
	public class ReceptionsController : ControllerBase
	{
		private readonly PharmaContext _context;
		private readonly ClaimsPrincipal _caller;

		public ReceptionsController(PharmaContext context, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_caller = httpContextAccessor.HttpContext.User;

		}

		// GET: api/Receptions
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Reception>>> GetReceptions()
		{
			return await _context.Receptions.ToListAsync();
		}

		// GET: api/Receptions/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Reception>> GetReception(int id)
		{
			var reception = await _context.Receptions.FindAsync(id);

			if (reception == null)
			{
				return NotFound();
			}

			return reception;
		}

		[HttpGet("GetReceptionsByDoctorId/{doctorId}")]
		public async Task<ActionResult<IEnumerable<Reception>>> GetReceptionsByDoctorId([FromRoute] int doctorId)
		{
			return await _context.Receptions.Where(r => r.DoctorId == doctorId).ToListAsync();
		}

		[HttpGet("GetReceptionsByPatientId/{patientId}")]
		public async Task<ActionResult<IEnumerable<Reception>>> GetReceptionsByPatientId([FromRoute] int patientId)
		{
			return await _context.Receptions.Where(r => r.PatientId == patientId).ToListAsync();
		}

		[HttpGet("GetAvailableReceptionTime/{doctorId}/{date}")]
		public ActionResult<IEnumerable<string>> GetAvailableReceptionTime([FromRoute] int doctorId, [FromRoute] string date)
		{
			DateTime dateTime = DateTime.Parse(date);//2005-05-05
			var receptions = _context.Receptions.Where(r =>
				r.DoctorId == doctorId && r.Date.Year == dateTime.Year && r.Date.Month == dateTime.Month &&
				r.Date.Day == dateTime.Day).ToList();

			var hours = Utils.GetWorkingHours();

			//var hours = new List<string>
			//{
			//	"08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30",
			//	"13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00"
			//};

			foreach (var r in receptions)
			{
				if (hours.Contains(r.Time.ToString()))
				{
					hours.Remove(r.Time.ToString());
				}
			}

			return Ok(hours);
		}

		// PUT: api/Receptions/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutReception(int id, Reception reception)
		{
			if (id != reception.ReceptionId)
			{
				return BadRequest();
			}

			_context.Entry(reception).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ReceptionExists(id))
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

		// POST: api/Receptions
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPost]
		public async Task<ActionResult<Reception>> PostReception(ReceptionViewModel receptionViewModel)
		{
			var reception = receptionViewModel.ToReception();
			if (_context.Receptions.ToList().Any())
			{
				var doctorsReceptions = _context.Receptions.Where(r =>
					r.DoctorId == reception.DoctorId &&
					DateTime.Compare(r.Date.Date, reception.Date.Date) == 0 &&
					!((r.Time < reception.Time && r.Time + r.Duration > reception.Time) ||
					  (reception.Time < r.Time && reception.Time + reception.Duration > r.Time))
				).ToList();

				if (doctorsReceptions.Any())
				{
					return BadRequest("Reception of this time is already set");
				}
			}

			reception.PatientId = Utils.GetPatient(_caller, _context).PatientId;
			reception.DayOfWeek = reception.Date.DayOfWeek.ToString();
			_context.Receptions.Add(reception);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetReception", new { id = reception.ReceptionId }, reception);
		}

		// DELETE: api/Receptions/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<Reception>> DeleteReception(int id)
		{
			var reception = await _context.Receptions.FindAsync(id);
			if (reception == null)
			{
				return NotFound();
			}

			_context.Receptions.Remove(reception);
			await _context.SaveChangesAsync();

			return reception;
		}

		public IEnumerable<Reception> GetRelevantDoctorsReceptions(int doctorId)
		{
			return _context.Receptions.Where(r => r.DoctorId == doctorId && DateTime.Compare(r.Date, DateTime.Now) >= 0);
		}

		private bool ReceptionExists(int id)
		{
			return _context.Receptions.Any(e => e.ReceptionId == id);
		}
		}
}
