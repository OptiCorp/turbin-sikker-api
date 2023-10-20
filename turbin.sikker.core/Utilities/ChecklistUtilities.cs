using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistDtos;
using turbin.sikker.core.Model.DTO.WorkflowDtos;

namespace turbin.sikker.core.Utilities
{
public class ChecklistUtilities : IChecklistUtilities
	{
        public bool checklistExists(IEnumerable<ChecklistResponseDto> checklists, string userId, string title)
        {
            return checklists.Any(c => c.User.Id == userId && c.Title == title);
        }

        public ChecklistResponseDto ChecklistToResponseDto(Checklist checklist)
        {    
            int? completionTime = 0;
            foreach(var task in checklist.ChecklistTasks)
            {
                completionTime += task.EstAvgCompletionTime;
            }
            
            return new ChecklistResponseDto
            {
                Id = checklist.Id,
                Title = checklist.Title,
                User = checklist.Creator,
                Status = checklist.Status == ChecklistStatus.Inactive ? "Inactive" : "Active",
                CreatedDate = checklist.CreatedDate,
                UpdatedDate = checklist.UpdatedDate,
                ChecklistTasks = checklist.ChecklistTasks,
                Workflows = new List<WorkflowResponseDto>(),
                EstCompletionTimeMinutes = completionTime
            };
        }

        public ChecklistResponseNoUserDto ChecklistToNoUserDto(Checklist checklist)
        {   
            int? completionTime = 0;
            foreach(var task in checklist.ChecklistTasks)
            {
                completionTime += task.EstAvgCompletionTime;
            }

            return new ChecklistResponseNoUserDto
            {
                Id = checklist.Id,
                Title = checklist.Title,
                Status = checklist.Status == ChecklistStatus.Inactive ? "Inactive" : "Active",
                CreatedDate = checklist.CreatedDate,
                UpdatedDate = checklist.UpdatedDate,
                ChecklistTasks = checklist.ChecklistTasks,
                Workflows = new List<WorkflowResponseDto>(),
                EstCompletionTimeMinutes = completionTime
            };
        }
    }
}