using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model
{
	public class Upload
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

		public string PunchId { get; set; }

		public string BlobRef { get; set; }
	}
}

