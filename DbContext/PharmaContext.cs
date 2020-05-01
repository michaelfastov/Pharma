using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pharma.DbContext.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Pharma.DbContext
{
	public class PharmaContext : IdentityDbContext<AppUser>
	{
		public PharmaContext(DbContextOptions<PharmaContext> options) : base(options) { }

		public virtual DbSet<Patient> Patients { get; set; }
		public virtual DbSet<Doctor> Doctors { get; set; }
		public virtual DbSet<Hospital> Hospitals { get; set; }
		public virtual DbSet<Reception> Receptions { get; set; }
		public virtual DbSet<Document> Documents { get; set; }
		public virtual DbSet<Review> Reviews { get; set; }
		public virtual DbSet<DoctorRating> DoctorRatings { get; set; }
		public virtual DbSet<DoctorToDoctorRating> DoctorToDoctorRatings { get; set; }
		public virtual DbSet<Relative> Relatives { get; set; }
		public virtual DbSet<Drug> Drugs { get; set; }
		public virtual DbSet<Procedure> Procedures { get; set; }
	}
}
