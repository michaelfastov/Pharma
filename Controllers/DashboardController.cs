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

namespace Pharma.Controllers
{
	//[Authorize(AuthenticationSchemes = "Bearer", Policy = "ApiUser")]
	[Route("api/[controller]/[action]")]
	public class DashboardController : Controller
	{
		private readonly ClaimsPrincipal _caller;
		private readonly PharmaContext _appDbContext;

		public DashboardController(UserManager<AppUser> userManager, PharmaContext appDbContext, IHttpContextAccessor httpContextAccessor)
		{
			_caller = httpContextAccessor.HttpContext.User;
			_appDbContext = appDbContext;
		}

		// GET api/dashboard/home
		[HttpGet]
		public async Task<IActionResult> Home()
		{
			// retrieve the user info
			//HttpContext.User
			var userId = _caller.Claims.Single(c => c.Type == "id");
			var patient = await _appDbContext.Patients.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);

			return new OkObjectResult(new
			{
				Message = "This is secure API and user data!",
				patient.Name,
				patient.Surname,
				patient.Identity.PictureUrl,
				patient.Identity.FacebookId,
			});
		}
	}
}