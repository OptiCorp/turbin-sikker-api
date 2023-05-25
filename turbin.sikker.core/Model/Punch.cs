using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace turbin.sikker.core.Model
{
	enum Severity { Minor, Major, Critical }
	enum Status { Pending, Approved, Rejected }
		
	public class Punch
	{
		public string Id { get; set; }
				
		public string Form_Id { get; set; }

        public string User_Id { get; set; }

		//public DateOnly CreatedDate { get; set; }

		[Column("punch_description")]
        public string PunchDescription { get; set; }

		public int Severity { get; set; }

		[Column("punch_status")]
		public int PunchStatus { get; set; }

		public Byte Active { get; set; }
	}
}

