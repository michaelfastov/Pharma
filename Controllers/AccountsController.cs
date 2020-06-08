using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharma.DbContext;
using Pharma.DbContext.Entities;
using Pharma.Helpers;
using Pharma.Models;
using Pharma.ViewModels;

namespace Pharma.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly PharmaContext _appDbContext;
		private readonly UserManager<AppUser> _userManager;
		private readonly IMapper _mapper;

		private readonly PharmaContext _context;
		private readonly ClaimsPrincipal _caller;

		public AccountsController(UserManager<AppUser> userManager, IMapper mapper, PharmaContext appDbContext, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_mapper = mapper;
			_appDbContext = appDbContext;
			_caller = httpContextAccessor.HttpContext.User;
		}

		// POST api/accounts
		[HttpPost]
		public async Task<IActionResult> Post([FromBody]RegistrationViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var userIdentity = _mapper.Map<AppUser>(model);

			var result = await _userManager.CreateAsync(userIdentity, model.Password);

			if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

			await _appDbContext.Patients.AddAsync(new Patient { IdentityId = userIdentity.Id });
			await _appDbContext.SaveChangesAsync();

			return new OkObjectResult("Account created");
		}

		[HttpPost("PostDoctor")]
		public async Task<IActionResult> PostDoctor([FromBody]RegistrationViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var userIdentity = _mapper.Map<AppUser>(model);

			var result = await _userManager.CreateAsync(userIdentity, model.Password);

			if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

			await _appDbContext.Doctors.AddAsync(new Doctor { IdentityId = userIdentity.Id });
			await _appDbContext.SaveChangesAsync();

			return new OkObjectResult("Doctor account created");
		}

		[Authorize(AuthenticationSchemes = "Bearer", Policy = "ApiUser")]
		[HttpPost("PutPatient")]
		public async Task<IActionResult> PutPatient(UpdatePatient patient)
		{
			var savedPatient = Utils.GetPatient(_caller, _appDbContext);
			savedPatient.Name = patient.Name;
			savedPatient.Surname = patient.Surname;
			savedPatient.Address = patient.Address;
			savedPatient.Phone = patient.Phone;

			_appDbContext.Entry(savedPatient).State = EntityState.Modified;

			await _appDbContext.SaveChangesAsync();

			return NoContent();
		}
	}
}