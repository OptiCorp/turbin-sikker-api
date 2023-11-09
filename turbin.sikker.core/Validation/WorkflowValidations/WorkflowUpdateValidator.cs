using FluentValidation;
using turbin.sikker.core.Model.DTO.WorkflowDtos;

namespace turbin.sikker.core.Validation.WorkflowValidations
{
    public class WorkflowUpdateValidator : AbstractValidator<WorkflowUpdateDto>
    {
        public WorkflowUpdateValidator()
        {
            RuleFor(workflow => workflow.Id).NotEmpty().WithMessage("You must specify a ID for this workflow").NotNull().WithMessage("You must specify a ID for this workflow");

            RuleFor(workflow => workflow.Status).Must((status) => status == "Sent" || status == "Committed" || status == "Done" || status == "Rejected" || status == null).WithMessage("Status must be Sent, Committed or Done");
        }
    }
}

