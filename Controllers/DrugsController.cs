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
	public class DrugsController : ControllerBase
	{
		private readonly PharmaContext _context;

		public DrugsController(PharmaContext context)
		{
			_context = context;
		}

		// GET: api/Drugs
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Drug>>> GetDrugs()
		{
			return await _context.Drugs.ToListAsync();
		}

		[HttpGet("GetDrugsByPatientId/{patientId}")]
		public async Task<ActionResult<IEnumerable<Drug>>> GetDrugsByPatientId([FromRoute] int patientid)
		{
			return await _context.Drugs.Where(d => d.PatientId == patientid).ToListAsync();
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

		// POST: api/Drugs
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPost]
		public async Task<ActionResult<Drug>> PostDrug(Drug drug)
		{
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
