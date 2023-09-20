using FluentValidation;
using turbin.sikker.core.Model.DTO.WorkflowDtos;

namespace turbin.sikker.core.Validation.WorkflowValidations
{
    public class WorkflowCreateValidator : AbstractValidator<WorkflowCreateDto>
    {
        public WorkflowCreateValidator()
        {
            RuleFor(workflow => workflow.ChecklistId).NotEmpty().WithMessage("You must specify a checklist for this workflow").NotNull().WithMessage("You must specify a checklist for this workflow");

            RuleFor(worklflow => worklflow.UserIds).NotEmpty().WithMessage("You must sepcify inspectors for this workflow").NotNull().WithMessage("You must sepcify inspectors for this workflow");

            RuleFor(worklflow => worklflow.CreatorId).NotEmpty().WithMessage("You must specify a sender for this workflow").NotNull().WithMessage("You must specify a sender for this workflow");
        }
    }
}

