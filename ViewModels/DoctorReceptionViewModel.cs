using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pharma.DbContext.Entities;

namespace Pharma.ViewModels
{
	public class DoctorReceptionViewModel: Reception
	{
		public string PatientName { get; set; }
	}
}
