using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma.DbContext.Entities
{
	public class Review
	{
		public int ReviewId { get; set; }
		public int ReceptionId { get; set; }
		public string Header { get; set; }
		public string Body { get; set; }

		public virtual Reception Reception { get; set; }
	}
}
