using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pharma.DbContext;
using Pharma.DbContext.Entities;
using Pharma.Helpers;
using Pharma.Models.LiqPay;
using Pharma.ViewModels;

namespace Pharma.Controllers
{
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

		[HttpGet("GetLiqPayModel/{doctorId}")]
		public LiqPayCheckoutFormModel GetLiqPayModel(int doctorId)
		{
			return LiqPayHelper.GetLiqPayModel(Guid.NewGuid().ToString(), Convert.ToInt32(_context.Doctors.First(d => d.DoctorId == doctorId).ReceptionPrice));
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

		[Authorize(AuthenticationSchemes = "Bearer", Policy = "ApiUser")]
		[HttpGet("GetPatientsReceptions")]
		public ActionResult<IEnumerable<PatientReceptionViewModel>> GetPatientsReceptions()
		{
			var patient = Utils.GetPatient(_caller, _context);

			var result =
				from reception in _context.Receptions
				join doctor in _context.Doctors on reception.DoctorId equals doctor.DoctorId
				where reception.PatientId == patient.PatientId
				orderby reception.Date
				select new PatientReceptionViewModel
				{
					ReceptionId = reception.ReceptionId,
					PatientId = reception.PatientId,
					DoctorId = reception.DoctorId,
					HospitalId = reception.HospitalId,
					Time = reception.Time,
					Duration = reception.Duration,
					FormatedDate = $"{reception.Date.Day}/{reception.Date.Month}/{reception.Date.Year}",
					DayOfWeek = reception.DayOfWeek,
					Address = reception.Address,
					Purpose = reception.Purpose,
					Result = reception.Result,
					Price = reception.Price,
					Name = doctor.Name,
					PaymentId = reception.PaymentId,
					IsPayed = reception.IsPayed
				};

			var receptions = result.ToList();
			for (var i = 0; i < receptions.Count; i++)
			{
				var paymentId = Guid.NewGuid().ToString();
				var liqPayModel = LiqPayHelper.GetLiqPayModel(paymentId, Convert.ToInt32(receptions[i].Price));
				receptions[i].Data = liqPayModel.Data;
				receptions[i].Signature = liqPayModel.Signature;

				var receptionToUpdate = _context.Receptions.Find(receptions[i].ReceptionId);
				receptionToUpdate.PaymentId = paymentId;
				_context.Entry(receptionToUpdate).State = EntityState.Modified;
			}

			_context.SaveChangesAsync();

			return Ok(receptions);
		}

		[Authorize(AuthenticationSchemes = "Bearer", Policy = "ApiDoctor")]
		[HttpGet("GetDoctorsReceptions")]
		public ActionResult<IEnumerable<UserReceptionViewModel>> GetDoctorsReceptions()
		{
			var doctor = Utils.GetDoctor(_caller, _context);

			var result =
				from reception in _context.Receptions
				join patient in _context.Patients on reception.PatientId equals patient.PatientId
				where reception.DoctorId == doctor.DoctorId
				orderby reception.Date
				select new UserReceptionViewModel
				{
					ReceptionId = reception.ReceptionId,
					PatientId = reception.PatientId,
					DoctorId = reception.DoctorId,
					HospitalId = reception.HospitalId,
					Time = reception.Time,
					Duration = reception.Duration,
					FormatedDate = $"{reception.Date.Day}/{reception.Date.Month}/{reception.Date.Year}",
					DayOfWeek = reception.DayOfWeek,
					Address = reception.Address,
					Purpose = reception.Purpose,
					Result = reception.Result,
					Price = reception.Price,
					Name = patient.Name,
					IsPayed = reception.IsPayed
				};

			return Ok(result);
		}

		[HttpGet("GetAvailableReceptionTime/{doctorId}/{date}")]
		public ActionResult<IEnumerable<string>> GetAvailableReceptionTime([FromRoute] int doctorId, [FromRoute] string date)
		{
			DateTime dateTime = DateTime.Parse(date);//2005-05-05
			var receptions = _context.Receptions.Where(r =>
				r.DoctorId == doctorId && r.Date.Year == dateTime.Year && r.Date.Month == dateTime.Month &&
				r.Date.Day == dateTime.Day).ToList();

			var hours = Utils.GetWorkingHours();

			foreach (var r in receptions)
			{
				if (hours.Contains(r.Time.ToString()))
				{
					hours.Remove(r.Time.ToString());
				}
			}

			return Ok(hours);
		}

		[HttpPost("UpdatePayedReception")]
		public ActionResult UpdatePayedReception()
		{
			var requestDictionary = Request.Form.Keys.ToDictionary(key => key, key => Request.Form[key]);

			var requestData = Convert.FromBase64String(requestDictionary["data"]);
			var decodedString = Encoding.UTF8.GetString(requestData);
			var requestDataDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedString);

			var signature = LiqPayHelper.GetLiqPaySignature(requestDictionary["data"]);

			if (signature != requestDictionary["signature"])
				return BadRequest();

			if (requestDataDictionary["status"] == "sandbox" || requestDataDictionary["status"] == "success")
			{
				var receptionToUpdate = _context.Receptions.FirstOrDefault(r => r.PaymentId == requestDataDictionary["order_id"]);
				if (receptionToUpdate != null)
				{
					receptionToUpdate.IsPayed = true;
					_context.Entry(receptionToUpdate).State = EntityState.Modified;
					_context.SaveChangesAsync();

				}
			}

			return Ok(new object());
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
		[Authorize(AuthenticationSchemes = "Bearer", Policy = "ApiUser")]
		[HttpPost]
		public async Task<ActionResult<Reception>> PostReception(ReceptionViewModel receptionViewModel)
		{
			var reception = receptionViewModel.ToReception();
			if (_context.Receptions.Where(r => r.DoctorId == reception.DoctorId).ToList().Any())
			{
				var doctorsReceptions = new List<Reception>();

				foreach (var r in _context.Receptions.Where(r => r.DoctorId == reception.DoctorId).ToList())
				{
					if (r.DoctorId == reception.DoctorId &&
						DateTime.Compare(r.Date.Date, reception.Date.Date) == 0 &&
						((r.Time <= reception.Time && r.Time + r.Duration >= reception.Time) ||
						(reception.Time <= r.Time && reception.Time + reception.Duration >= r.Time)))
					{
						doctorsReceptions.Add(r);
					}
				}

				if (doctorsReceptions.Any())
				{
					return BadRequest("Reception of this time is already set");
				}
			}

			reception.IsPayed = false;
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
