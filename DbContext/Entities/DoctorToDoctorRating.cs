using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.DbContext.Entities
{
	public class DoctorToDoctorRating
	{
		public int DoctorToDoctorRatingId { get; set; }
		public int DoctorId { get; set; }
		public int DoctorRatingId { get; set; }

		public Doctor Doctor { get; set; }
		public DoctorRating DoctorRating { get; set; }
	}
}
