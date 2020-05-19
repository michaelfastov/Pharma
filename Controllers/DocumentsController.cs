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
	public class DocumentsController : ControllerBase
	{
		private readonly PharmaContext _context;
		private readonly ClaimsPrincipal _caller;

		public DocumentsController(PharmaContext context, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_caller = httpContextAccessor.HttpContext.User;
		}

		// GET: api/Documents
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
		{
			return await _context.Documents.ToListAsync();
		}

		// GET: api/Documents/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Document>> GetDocument(int id)
		{
			var document = await _context.Documents.FindAsync(id);

			if (document == null)
			{
				return NotFound();
			}

			return document;
		}

		// PUT: api/Documents/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutDocument(int id, Document document)
		{
			if (id != document.DocumentId)
			{
				return BadRequest();
			}

			_context.Entry(document).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!DocumentExists(id))
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
		public async Task<ActionResult<Document>> PostDocument(Document document)
		{
			var file = document.File;
			var index = document.File.IndexOf("base64,", StringComparison.Ordinal);
			var base64String = document.File.Substring(index + 7);
			document.File = base64String;

			var doctor = Utils.GetDoctor(_caller, _context);
			document.DoctorId = doctor.DoctorId;
			_context.Documents.Add(document);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetDocument", new { id = document.DocumentId }, document);
		}

		[HttpGet("GetDocumentsByPatientId/{patientId}")]
		public async Task<ActionResult<IEnumerable<Document>>> GetDocumentsByPatientId([FromRoute] int patientId)
		{
			return await _context.Documents.Where(d => d.PatientId == patientId).ToListAsync();
		}

		[Authorize(AuthenticationSchemes = "Bearer", Policy = "ApiUser")]
		[HttpGet("GetPatientDocuments")]
		public ActionResult<IEnumerable<PatientDocumentViewModel>> GetPatientDocuments()
		{
			var patient = Utils.GetPatient(_caller, _context);
			var result =
				from document in _context.Documents
				join doctors in _context.Doctors on document.DoctorId equals doctors.DoctorId
				orderby doctors.Name
				select new PatientDocumentViewModel
				{
					DocumentId = document.DocumentId,
					PatientId = document.PatientId,
					DoctorId = doctors.DoctorId,
					DoctorName = doctors.Name,
					Name = document.Name,
					Comments = document.Comments,
					File = document.File,
				};

			return Ok(result);
		}

		[HttpGet("GetFileByDocumentId/{documentId}")]
		public FileResult GetFileByDocumentId(int documentId)
		{
			var document = _context.Documents.First(d => d.DocumentId == documentId);
			return File(Convert.FromBase64String(document.File), "application/pdf", document.Name);
		}

		// DELETE: api/Documents/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<Document>> DeleteDocument(int id)
		{
			var document = await _context.Documents.FindAsync(id);
			if (document == null)
			{
				return NotFound();
			}

			_context.Documents.Remove(document);
			await _context.SaveChangesAsync();

			return document;
		}

		private bool DocumentExists(int id)
		{
			return _context.Documents.Any(e => e.DocumentId == id);
		}
	}
}
