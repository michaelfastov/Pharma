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
	[Route("api/[controller]")]
	[ApiController]
	public class DrugsController : ControllerBase
	{
		private readonly PharmaContext _context;
		private readonly ClaimsPrincipal _caller;

		public DrugsController(PharmaContext context, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_caller = httpContextAccessor.HttpContext.User;
		}

		// GET: api/Drugs
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Drug>>> GetDrugs()
		{
			return await _context.Drugs.ToListAsync();
		}

		[HttpGet("GetDrugsByPatientId/{patientId}")]
		public async Task<ActionResult<IEnumerable<Drug>>> GetDrugsByPatientId([FromRoute] int patientId)
		{
			return await _context.Drugs.Where(d => d.PatientId == patientId).ToListAsync();
		}


		[Authorize(AuthenticationSchemes = "Bearer", Policy = "ApiUser")]
		[HttpGet("GetDrugsByDoctorId/{doctorId}")]
		public async Task<ActionResult<IEnumerable<Drug>>> GetDrugsByDoctorId([FromRoute] int doctorId)
		{
			var patient = Utils.GetPatient(_caller, _context);
			return await _context.Drugs.Where(d => d.DoctorId == doctorId && d.PatientId == patient.PatientId).ToListAsync();
		}

		[Authorize(AuthenticationSchemes = "Bearer", Policy = "ApiUser")]
		[HttpGet("GetPatientsDrugs")]
		public ActionResult<IEnumerable<PatientDrugsViewModel>> GetPatientsDrugs()
		{
			var patient = Utils.GetPatient(_caller, _context);
			var result =
				from drugs in _context.Drugs
				join doctors in _context.Doctors on drugs.DoctorId equals doctors.DoctorId
				orderby doctors.Name
				select new PatientDrugsViewModel
				{
					DrugId = drugs.DrugId,
					PatientId = drugs.PatientId,
					DoctorId = doctors.DoctorId,
					DoctorName = doctors.Name,
					Name = drugs.Name,
					Price = drugs.Price,
					Category = drugs.Category,
					Comments = drugs.Comments
				};

			return Ok(result);
		}

		// GET: api/Drugs/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Drug>> GetDrug(int id)
		{
			var drug = await _context.Drugs.FindAsync(id);

			if (drug == null)
			{
				return NotFound();
			}

			return drug;
		}

		// PUT: api/Drugs/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutDrug(int id, Drug drug)
		{
			if (id != drug.DrugId)
			{
				return BadRequest();
			}

			_context.Entry(drug).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!DrugExists(id))
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

		[Authorize(AuthenticationSchemes = "Bearer", Policy = "ApiDoctor")]
		[HttpPost]
		public async Task<ActionResult<Drug>> PostDrug(Drug drug)
		{
			var doctor = Utils.GetDoctor(_caller, _context);
			drug.DoctorId = doctor.DoctorId;
			_context.Drugs.Add(drug);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetDrug", new { id = drug.DrugId }, drug);
		}

		// DELETE: api/Drugs/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<Drug>> DeleteDrug(int id)
		{
			var drug = await _context.Drugs.FindAsync(id);
			if (drug == null)
			{
				return NotFound();
			}

			_context.Drugs.Remove(drug);
			await _context.SaveChangesAsync();

			return drug;
		}

		private bool DrugExists(int id)
		{
			return _context.Drugs.Any(e => e.DrugId == id);
		}
	}
}
