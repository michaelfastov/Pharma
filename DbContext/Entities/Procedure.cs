using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.DbContext.Entities
{
	public class Procedure
	{
		public int ProcedureId { get; set; }
		public int PatientId { get; set; }
		public int DoctorId { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public string Category { get; set; }
		public string Comments { get; set; }

		public Patient Patient { get; set; }
		public Doctor Doctor { get; set; }

	}
}
