using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pharma.DbContext.Entities;
using Pharma.Helpers;

namespace Pharma.ViewModels
{
	public class ReceptionViewModel
	{
		public int ReceptionId { get; set; }
		public int PatientId { get; set; }
		public int DoctorId { get; set; }
		public int HospitalId { get; set; }
		public string Time { get; set; }
		public string Duration { get; set; }
		public DateTime Date { get; set; }
		public string DayOfWeek { get; set; }
		public string Address { get; set; }
		public string Purpose { get; set; }
		public string Result { get; set; }
		public int Price { get; set; }

		public Reception ToReception()
		{
			return new Reception
			{
				ReceptionId = this.ReceptionId,
				PatientId = this.PatientId,
				DoctorId = this.DoctorId,
				HospitalId = this.HospitalId,
				Time = Utils.GetTimeSpanFromString(this.Time),
				Duration = Utils.GetTimeSpanFromString(this.Duration),
				Date = this.Date,
				DayOfWeek = this.DayOfWeek,
				Address = this.Address,
				Purpose = this.Purpose,
				Result = this.Result,
				Price = this.Price
			};
		}
	}
}
