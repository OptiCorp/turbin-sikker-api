using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.WorkflowDtos;

namespace turbin.sikker.core.Utilities
{
public class WorkflowUtilities : IWorkflowUtilities
	{
        public WorkflowResponseDto WorkflowToResponseDto(Workflow? workflow)
        {   
            var taskInfos = new Dictionary<string, string>();
            foreach(var info in workflow.TaskInfos)
            {
                taskInfos.Add(info.TaskId, info.Status.ToString());
            }
            return new WorkflowResponseDto
            {
                Id = workflow.Id,
                Checklist = workflow.Checklist,
                User = workflow.User,
                Creator = workflow.Creator,
                Status = workflow.Status.ToString(),
                CreatedDate = workflow.CreatedDate,
                UpdatedDate = workflow.UpdatedDate,
                InvoiceId = workflow.InvoiceId,
                CompletionTimeMinutes = workflow.CompletionTimeMinutes,
                TaskInfos = taskInfos
            };
        }
    }
}