using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model
{
	public enum CStatus { Active = 0, Inactive = 1}

	public class Checklist
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

		[Required]
        [StringLength(50)]
        public string Title { get; set; }

		public CStatus ChecklistStatus { get; set; }

		public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string CreatedBy { get; set; }

        public User? CreatedByUser { get; }
        public ICollection<ChecklistTask>? ChecklistTasks { get;  }

    }
}

