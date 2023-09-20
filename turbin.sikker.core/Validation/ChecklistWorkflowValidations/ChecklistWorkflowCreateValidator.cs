using FluentValidation;
using turbin.sikker.core.Migrations;
using turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos;

namespace turbin.sikker.core.Validation.ChecklistValidations
{
    public class ChecklistWorkflowCreateValidator : AbstractValidator<ChecklistWorkflowCreateDto>
    {
        public ChecklistWorkflowCreateValidator()
        {
            RuleFor(checklistWorkflow => checklistWorkflow.ChecklistId).NotEmpty().WithMessage("You must specify a checklist for this workflow").NotNull().WithMessage("You must specify a checklist for this workflow");

            RuleFor(checklistWorklflow => checklistWorklflow.UserIds).NotEmpty().WithMessage("You must sepcify inspectors for this workflow").NotNull().WithMessage("You must sepcify inspectors for this workflow");

            RuleFor(checklistWorklflow => checklistWorklflow.CreatorId).NotEmpty().WithMessage("You must specify a sender for this workflow").NotNull().WithMessage("You must specify a sender for this workflow");
        }
    }
}

