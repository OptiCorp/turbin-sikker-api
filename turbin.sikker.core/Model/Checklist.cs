using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model
{
	public enum CStatus { Pending = 0, Approved = 1, Rejected = 2 }

	public class Checklist
	{

		public string Id { get; set; }

		[Required]
        [StringLength(50)]
        public string Title { get; set; }

		public CStatus ChecklistStatus { get; set; }

		public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string CreatedBy { get; set; }

        public User? CreatedByUser { get; }
        public ICollection<ChecklistTask>? ChecklistTasks { get;  }

    }
}

