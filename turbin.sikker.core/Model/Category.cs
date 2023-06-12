using System;
using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model
{
	
	public class Category
	{
		public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

	}
}

