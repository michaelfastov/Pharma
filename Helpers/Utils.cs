using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pharma.DbContext;
using Pharma.DbContext.Entities;

namespace Pharma.Helpers
{
	public class Utils
	{
		public static TimeSpan GetTimeSpanFromString(string stringTimeSpan)
		{
			var times = stringTimeSpan.Split(":").Select(Int32.Parse).ToList();
			return new TimeSpan(times[0], times[1], times[2]);
		}

		public static List<string> GetWorkingHours()
		{
			return new List<string>
			{
				"08:00:00",
				"08:30:00",
				"09:00:00",
				"09:30:00",
				"10:00:00",
				"10:30:00",
				"11:00:00",
				"11:30:00",
				"12:00:00",
				"12:30:00",
				"13:00:00",
				"13:30:00",
				"14:00:00",
				"14:30:00",
				"15:00:00",
				"15:30:00",
				"16:00:00"
			};
		}

		public static Patient GetPatient(ClaimsPrincipal _caller, PharmaContext _context)
		{
			var test = _caller.Claims.ToList();
			var userId = _caller.Claims.Single(c => c.Type == "id");
			var patient = _context.Patients.Include(c => c.Identity).Single(c => c.Identity.Id == userId.Value);

			return patient;
		}

		public static Doctor GetDoctor(ClaimsPrincipal _caller, PharmaContext _context)
		{
			var test = _caller.Claims.ToList();
			var userId = _caller.Claims.Single(c => c.Type == "id");
			var doctor = _context.Doctors.Include(c => c.Identity).Single(c => c.Identity.Id == userId.Value);

			return doctor;
		}
	}
}
