using Microsoft.VisualBasic;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.WorkflowDtos;

namespace turbin.sikker.core.Utilities
{
    public class WorkflowUtilities : IWorkflowUtilities
    {
        private readonly IChecklistUtilities _checklistUtilities;

        public WorkflowUtilities(IChecklistUtilities checklistUtilities)
        {
            _checklistUtilities = checklistUtilities;
        }
        public WorkflowResponseDto WorkflowToResponseDto(Workflow? workflow)
        {
            if (workflow == null)
            {
                return null;
            }
            var taskInfos = new Dictionary<string, string>();
            if (workflow.TaskInfos != null)
            {
                foreach (var info in workflow.TaskInfos)
                {
                    taskInfos.Add(info.TaskId, info.Status.ToString());
                }
            }
            return new WorkflowResponseDto
            {
                Id = workflow.Id,
                Checklist = _checklistUtilities.ChecklistInWorkflowToResponseDto(workflow.Checklist),
                User = workflow.User,
                Creator = workflow.Creator,
                Status = workflow.Status.ToString(),
                CreatedDate = workflow.CreatedDate,
                UpdatedDate = workflow.UpdatedDate,
                InvoiceId = workflow.InvoiceId,
                CompletionTimeMinutes = workflow.CompletionTimeMinutes,
                TaskInfos = taskInfos,
                Comment = workflow.Comment
            };
        }
    }
}
