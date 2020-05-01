using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.DbContext.Entities
{
	public class Drug
	{
		public int DrugId { get; set; }
		public int PatientId { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public string Category { get; set; }

		public Patient Patient { get; set; }
	}
}
