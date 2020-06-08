using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.Models
{
	public class UpdatePatient
	{
		public int PatientId { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
	}
}
