using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharma.DbContext;
using Pharma.DbContext.Entities;
using Pharma.Helpers;
using Pharma.ViewModels;

namespace Pharma.Controllers
{
	//[Authorize(AuthenticationSchemes = "Bearer", Policy = "ApiUser")]
	[Route("api/[controller]")]
	[ApiController]

	public class DashboardController : ControllerBase
	{
		private readonly ClaimsPrincipal _caller;
		private readonly PharmaContext _appDbContext;

		public DashboardController(UserManager<AppUser> userManager, PharmaContext appDbContext, IHttpContextAccessor httpContextAccessor)
		{
			_caller = httpContextAccessor.HttpContext.User;
			_appDbContext = appDbContext;
		}

		[Authorize(AuthenticationSchemes = "Bearer", Policy = "ApiUser")]
		[HttpGet("GetPatientHome")]
		public ActionResult<Patient> GetPatientHome()
		{
			var patient = Utils.GetPatient(_caller, _appDbContext);

			return Ok(new PatientViewModel
			{
				PatientId = patient.PatientId,
				Name = patient.Name,
				Surname = patient.Surname,
				FormatedDOB = $"{patient.DOB.Day}/{patient.DOB.Month}/{patient.DOB.Year}",
				Address = patient.Address,
				Phone = patient.Phone
			});
		}

		[Authorize(AuthenticationSchemes = "Bearer", Policy = "ApiDoctor")]
		[HttpGet("GetDoctorHome")]
		public ActionResult<Doctor> GetDoctorHome()
		{
			var doctor = Utils.GetDoctor(_caller, _appDbContext);

			return Ok(doctor);
		}
	}
}