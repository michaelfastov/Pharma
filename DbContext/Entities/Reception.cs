using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.DbContext.Entities
{
	public class Reception
	{
		public int ReceptionId { get; set; }
		public int PatientId { get; set; }
		public int DoctorId { get; set; }
		public int HospitalId { get; set; }
		public TimeSpan Time { get; set; }
		public TimeSpan Duration { get; set; }
		public DateTime Date { get; set; }
		public string DayOfWeek { get; set; }
		public string Address { get; set; }
		public string Purpose { get; set; }
		public string Result { get; set; }
		public int Price { get; set; }

		public virtual Patient Patient { get; set; }
		public virtual Doctor Doctor { get; set; }
		public virtual Hospital Hospital { get; set; }

		public virtual ICollection<Review> Review { get; set; }

	}
}
