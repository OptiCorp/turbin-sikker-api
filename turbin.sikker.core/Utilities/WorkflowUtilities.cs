using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.WorkflowDtos;

namespace turbin.sikker.core.Utilities
{
public class WorkflowUtilities : IWorkflowUtilities
	{
        public WorkflowResponseDto WorkflowToResponseDto(Workflow? workflow)
        {
            return new WorkflowResponseDto
            {
                Id = workflow.Id,
                Checklist = workflow.Checklist,
                User = workflow.User,
                Creator = workflow.Creator,
                Status = workflow.Status.ToString(),
                CreatedDate = workflow.CreatedDate,
                UpdatedDate = workflow.UpdatedDate
            };
        }
    }
}