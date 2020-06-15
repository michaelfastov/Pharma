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
    public class ProceduresController : ControllerBase
    {
        private readonly PharmaContext _context;
        private readonly ClaimsPrincipal _caller;

        public ProceduresController(PharmaContext context, IHttpContextAccessor httpContextAccessor)
        {
	        _context = context;
	        _caller = httpContextAccessor.HttpContext.User;
        }

        // GET: api/Procedures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Procedure>>> GetProcedures()
        {
            return await _context.Procedures.ToListAsync();
        }

        [HttpGet("GetProceduresByPatientId/{patientId}")]
        public async Task<ActionResult<IEnumerable<Procedure>>> GetProceduresByPatientId([FromRoute] int patientId)
        {
	        return await _context.Procedures.Where(d => d.PatientId == patientId).ToListAsync();
        }

        // GET: api/Procedures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Procedure>> GetProcedure(int id)
        {
            var procedure = await _context.Procedures.FindAsync(id);

            if (procedure == null)
            {
                return NotFound();
            }

            return procedure;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "ApiUser")]
        [HttpGet("GetPatientsProcedures")]
        public ActionResult<IEnumerable<PatientProceduresViewModel>> GetPatientsProcedures()
        {
	        var patient = Utils.GetPatient(_caller, _context);
	        var result =
		        from procedures in _context.Procedures
		        join doctors in _context.Doctors on procedures.DoctorId equals doctors.DoctorId
		        orderby doctors.Name
		        select new PatientProceduresViewModel
                {
			        ProcedureId = procedures.ProcedureId,
			        PatientId = procedures.PatientId,
			        DoctorId = doctors.DoctorId,
			        DoctorName = doctors.Name,
			        Name = procedures.Name,
			        Price = procedures.Price,
			        Category = procedures.Category,
                    Comments = procedures.Comments
		        };

	        return Ok(result);
        }

        // PUT: api/Procedures/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProcedure(int id, Procedure procedure)
        {
            if (id != procedure.ProcedureId)
            {
                return BadRequest();
            }

            _context.Entry(procedure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcedureExists(id))
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
        public async Task<ActionResult<Procedure>> PostProcedure(Procedure procedure)
        {
	        var doctor = Utils.GetDoctor(_caller, _context);
	        procedure.DoctorId = doctor.DoctorId;
            _context.Procedures.Add(procedure);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProcedure", new { id = procedure.ProcedureId }, procedure);
        }

        // DELETE: api/Procedures/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Procedure>> DeleteProcedure(int id)
        {
            var procedure = await _context.Procedures.FindAsync(id);
            if (procedure == null)
            {
                return NotFound();
            }

            _context.Procedures.Remove(procedure);
            await _context.SaveChangesAsync();

            return procedure;
        }

        private bool ProcedureExists(int id)
        {
            return _context.Procedures.Any(e => e.ProcedureId == id);
        }
    }
}
