using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.ViewModels
{
	public class PatientReceptionViewModel : UserReceptionViewModel
	{
		public string Data { get; set; }
		public string Signature { get; set; }
	}
}
