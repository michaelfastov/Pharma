using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.DbContext.Entities
{
	public class DoctorToHospital
	{
		public int DoctorToHospitalId { get; set; }
		public int DoctorId { get; set; }
		public int HospitalId { get; set; }

		public Doctor Doctor { get; set; }
		public Hospital Hospital { get; set; }
	}
}
