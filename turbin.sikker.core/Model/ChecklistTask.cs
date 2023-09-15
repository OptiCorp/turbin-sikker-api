using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model
{
	public class ChecklistTask
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        [Required]
		public string? CategoryId { get; set; }

        [Required]
		public string? Description { get; set; }

        public Category? Category { get; }

    }
}

