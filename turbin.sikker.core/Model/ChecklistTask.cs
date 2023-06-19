using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model
{
	public class ChecklistTask
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

		public string CategoryId { get; set; }

		public string Description { get; set; }

        public Category? Category { get; }

    }
}

