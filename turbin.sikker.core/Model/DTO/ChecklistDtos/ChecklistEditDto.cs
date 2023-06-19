﻿using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.ChecklistDtos
{
    public class ChecklistEditDto
    {

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [EnumDataType(typeof(ChecklistStatus))]
        public ChecklistStatus Status { get; set; }

    }
}
