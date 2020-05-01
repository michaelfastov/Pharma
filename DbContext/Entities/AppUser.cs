using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Pharma.DbContext.Entities
{
	public class AppUser : IdentityUser
	{
		public long? FacebookId { get; set; }
		public string PictureUrl { get; set; }
	}
}
