using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model
{
	public class Upload
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

		[Required]
		public string? PunchId { get; set; }

		public Punch? Punch { get; }

		[Required]
		public string? BlobRef { get; set; }
	}
}

