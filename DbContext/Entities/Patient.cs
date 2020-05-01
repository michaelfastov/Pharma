using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.DbContext.Entities
{
	public class Patient
	{
		public int PatientId { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public DateTime DOB { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string IdentityId { get; set; }
		public AppUser Identity { get; set; }

		public virtual ICollection<Reception> Receptions { get; set; }
		public virtual ICollection<Document> Documents { get; set; }
		public virtual ICollection<Relative> Relatives { get; set; }
		public virtual ICollection<Drug> Drugs { get; set; }
		public virtual ICollection<Procedure> Procedures { get; set; }
	}
}
