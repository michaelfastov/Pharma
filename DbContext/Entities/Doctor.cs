using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.DbContext.Entities
{
	public class Doctor
	{
		public int DoctorId { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Phone { get; set; }
		public string Specialization { get; set; }
		public double ReceptionPrice { get; set; }
		//public bool IsDoctor { get; set; }
		public string IdentityId { get; set; }
		public AppUser Identity { get; set; }

		public virtual ICollection<Reception> Receptions { get; set; }
		public virtual ICollection<Document> Documents { get; set; }
		public virtual ICollection<DoctorToDoctorRating> DoctorToDoctorRating { get; set; }
		public virtual ICollection<DoctorToHospital> DoctorToHospital { get; set; }
	}
}
