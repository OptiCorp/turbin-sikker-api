using System;
namespace turbin.sikker.core.Model.DTO
{
    public class PunchCreateDto
    {
        public string CreatedBy { get; set; }

        public string PunchDescrption { get; set; }

        public string ChecklistId { get; set; }

        public int Severity { get; set; }

        //public string UserId { get; set; }
    }
}

