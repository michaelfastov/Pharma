using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.DbContext.Entities
{
	public class Relative
	{
		public int RelativeId { get; set; }
		public int PatientId { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }

		public Patient Patient { get; set; }
	}
}
