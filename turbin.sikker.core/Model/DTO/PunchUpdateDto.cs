using System;
namespace turbin.sikker.core.Model.DTO
{
    public class PunchUpdateDto
    {
        public string PunchDescription { get; set; }

        public string ChecklistId { get; set; }

        public int Severity { get; set; }

        public string Status { get; set; }
    }
}

