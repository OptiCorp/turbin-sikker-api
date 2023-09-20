using FluentValidation;
using turbin.sikker.core.Migrations;
using turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos;

namespace turbin.sikker.core.Validation.ChecklistValidations
{
    public class ChecklistWorkflowUpdateValidator : AbstractValidator<ChecklistWorkflowUpdateDto>
    {
        public ChecklistWorkflowUpdateValidator()
        {
            RuleFor(checklistWorkflow => checklistWorkflow.Id).NotEmpty().WithMessage("You must specify a ID for this workflow").NotNull().WithMessage("You must specify a ID for this workflow");

            RuleFor(checklistWorkflow => checklistWorkflow.Status).Must((status) => status == "Sent" || status == "Committed" || status == "Done" || status == null).WithMessage("Status must be Sent, Committed or Done");
        }
    }
}

