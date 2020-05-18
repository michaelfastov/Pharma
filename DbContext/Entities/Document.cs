using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.DbContext.Entities
{
	public class Document
	{
		public int DocumentId { get; set; }
		public int PatientId { get; set; }
		public int DoctorId { get; set; }
		public string Name { get; set; }
		public string Comments { get; set; }
		public string File { get; set; }

		public virtual Patient Patient { get; set; }
		public virtual Doctor Doctor { get; set; }
	}
}
