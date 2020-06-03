using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pharma.DbContext.Entities;

namespace Pharma.ViewModels
{
	public class UserReceptionViewModel: Reception
	{
		public string Name { get; set; }
		public string FormatedDate { get; set; }
	}
}
