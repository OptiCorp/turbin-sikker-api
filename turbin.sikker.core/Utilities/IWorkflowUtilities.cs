using turbin.sikker.core.Model.DTO.WorkflowDtos;
using turbin.sikker.core.Model;


namespace turbin.sikker.core.Utilities
{
public interface IWorkflowUtilities
    {
        public WorkflowResponseDto WorkflowToResponseDto(Workflow? workflow);
    }
}