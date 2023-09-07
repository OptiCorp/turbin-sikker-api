using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos;

namespace turbin.sikker.core.Utilities
{
public class ChecklistWorkflowUtilities : IChecklistWorkflowUtilities
	{
        public ChecklistWorkflowResponseDto WorkflowToResponseDto(ChecklistWorkflow? checklistWorkflow)
        {
            return new ChecklistWorkflowResponseDto
            {
                Id = checklistWorkflow.Id,
                Checklist = checklistWorkflow.Checklist,
                User = checklistWorkflow.User,
                Creator = checklistWorkflow.Creator,
                Status = checklistWorkflow.Status.ToString(),
                CreatedDate = checklistWorkflow.CreatedDate,
                UpdatedDate = checklistWorkflow.UpdatedDate
            };
        }
    }
}