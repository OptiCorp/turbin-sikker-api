using System.ComponentModel.DataAnnotations;
namespace turbin.sikker.core.Model
{
	enum Severity { Minor, Major, Critical }
	enum Status { Pending, Approved, Rejected }
		
	public class Punch
	{
		public string Id { get; set; }

		[Required]
		[StringLength(50)]
		public string FormId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        //public DateOnly CreatedDate { get; set; }

        [Required]
        [StringLength(1500)]
        public string PunchDescription { get; set; }

		public int Severity { get; set; }

        public int PunchStatus { get; set; }

		public Byte Active { get; set; }
	}
}

