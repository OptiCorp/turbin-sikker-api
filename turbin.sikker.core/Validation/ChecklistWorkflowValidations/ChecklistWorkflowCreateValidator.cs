using FluentValidation;
using turbin.sikker.core.Migrations;
using turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos;

namespace turbin.sikker.core.Validation.ChecklistValidations
{
    public class ChecklistWorkflowCreateValidator : AbstractValidator<ChecklistWorkflowCreateDto>
    {
        public ChecklistWorkflowCreateValidator()
        {
            RuleFor(checklistWorkflow => checklistWorkflow.ChecklistId).NotEmpty().NotNull().WithMessage("Checklist ID is required.");

            RuleFor(checklistWorklflow => checklistWorklflow.UserIds).NotEmpty().NotNull().WithMessage("You have to sepcify inspectors for this workflow");

            RuleFor(checklistWorklflow => checklistWorklflow.CreatedById).NotEmpty().NotNull().WithMessage("You have to specify a sender");
        }
    }
}

