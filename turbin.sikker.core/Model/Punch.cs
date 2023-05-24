using System;
namespace turbin.sikker.core.Model
{
	enum Severity { Minor, Major, Critical }
	enum Status { Pending, Approved, Rejected }
	public class Punch
	{
		public int Id { get; set; }

		public Form Form  { get; }

		//public int GetFormId()
		//{
		//	return Form.Id;
		//}

        public int UserId { get; set; }

		//public DateOnly CreatedDate { get; set; }

        public string Description { get; set; }

		public Boolean Active { get; set; }
	}
}

