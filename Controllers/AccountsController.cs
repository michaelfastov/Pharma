using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pharma.DbContext;
using Pharma.DbContext.Entities;
using Pharma.Helpers;
using Pharma.ViewModels;

namespace Pharma.Controllers
{
	[Route("api/[controller]")]
	public class AccountsController : Controller
	{
		private readonly PharmaContext _appDbContext;
		private readonly UserManager<AppUser> _userManager;
		private readonly IMapper _mapper;

		public AccountsController(UserManager<AppUser> userManager, IMapper mapper, PharmaContext appDbContext)
		{
			_userManager = userManager;
			_mapper = mapper;
			_appDbContext = appDbContext;
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
	}
}