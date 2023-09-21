﻿
namespace turbin.sikker.core.Model.DTO.ChecklistDtos
{
    public class ChecklistResponseNoUserDto
    {   
        public string? Id { get; set; }

        public string? Title { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public ICollection<ChecklistTask>? ChecklistTasks { get; set; }
    }
}