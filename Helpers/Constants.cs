﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.Helpers
{
	public static class Constants
	{
		public static class Strings
		{
			public static class JwtClaimIdentifiers
			{
				public const string Rol = "rol", Id = "id";
			}

			public static class JwtClaims
			{
				public const string ApiAccess = "api_access";
				public const string ApiDoctorAccess = "api_doctor_access";
			}
		}
	}
}
