namespace turbin.sikker.core.Model.DTO
{
    public class PunchCreateDto
    {
        public string CreatedBy { get; set; }

        public string PunchDescription { get; set; }

        public string ChecklistWorkflowId { get; set; }
        public string ChecklistTaskId { get; set; }


        public string Severity { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}

