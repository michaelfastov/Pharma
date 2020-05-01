using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.DbContext.Entities
{
	public class DoctorRating
	{
		public int DoctorRatingId { get; set; }
		public string Name { get; set; }

		public virtual ICollection<DoctorToDoctorRating> DoctorToDoctorRating { get; set; }

	}
}
