using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.ViewModels
{
	public class DoctorRatingViewModel
	{
		public int DoctorToDoctorRatingId { get; set; }
		public int DoctorId { get; set; }
		public int DoctorRatingId { get; set; }
		public int RankingPlace { get; set; }
		public string DoctorName { get; set; }
	}
}
