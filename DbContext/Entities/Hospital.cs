using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.DbContext.Entities
{
	public class Hospital
	{
		public int HospitalId { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public TimeSpan OpensAt { get; set; }
		public TimeSpan ClosesAt { get; set; }

		public virtual ICollection<Reception> Receptions { get; set; }
	}
}
