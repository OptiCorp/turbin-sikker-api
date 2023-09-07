using turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos;
using turbin.sikker.core.Model;


namespace turbin.sikker.core.Utilities
{
public interface IChecklistWorkflowUtilities
    {
        public ChecklistWorkflowResponseDto WorkflowToResponseDto(ChecklistWorkflow? checklistWorkflow);
    }
}