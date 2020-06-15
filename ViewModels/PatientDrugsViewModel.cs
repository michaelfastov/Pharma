using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.ViewModels
{
	public class PatientDrugsViewModel
	{
		public int DrugId { get; set; }
		public int PatientId { get; set; }
		public int DoctorId { get; set; }
		public string DoctorName { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public string Category { get; set; }

		public string Comments { get; set; }

	}
}
