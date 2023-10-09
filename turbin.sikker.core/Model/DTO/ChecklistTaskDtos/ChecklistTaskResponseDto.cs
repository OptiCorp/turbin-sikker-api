
namespace turbin.sikker.core.Model.DTO.TaskDtos
{
    public class ChecklistTaskResponseDto
    {
        public string? Id { get; set; }

        public string? Description { get; set; }

        public Category? Category { get; set; }

        public int? EstAvgCompletionTime { get; set; }
    }
}
