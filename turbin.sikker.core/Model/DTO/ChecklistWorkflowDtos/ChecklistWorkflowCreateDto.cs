namespace turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos
{
    public class ChecklistWorkflowCreateDto
    {
        public string? ChecklistId { get; set; }

        public ICollection<string>? UserIds { get; set; }

        public string? CreatedById { get; set; }

        public string? Status { get; set; }

    }
}