using FluentValidation;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Validation.PunchValidations
{
    public class PunchCreateValidation : AbstractValidator<PunchCreateDto>
    {
        public PunchCreateValidation()
        {
            RuleFor(punch => punch.CreatorId).NotEmpty().WithMessage("You must specify a creator for this punch").NotNull().WithMessage("You must specify a creator for this punch");    
            
            RuleFor(punch => punch.Description).NotEmpty().WithMessage("Description is required.")
                .NotNull().WithMessage("Description cannot be null.")
                .MinimumLength(5).WithMessage("Description must be at least 5 characters.")
                .MaximumLength(500).WithMessage("Category name cannot exceed 500 characters.");

            RuleFor(punch => punch.WorkflowId).NotEmpty().WithMessage("You must specify a workflow ID for this punch").NotNull().WithMessage("You must specify a workflow ID for this punch");    

            RuleFor(punch => punch.ChecklistTaskId).NotEmpty().WithMessage("You must specify a task ID for this punch").NotNull().WithMessage("You must specify a task ID for this punch");    

            RuleFor(punch => punch.Severity).Must((severity) => severity == "Minor" || severity == "Major" || severity == "Critical" || severity == null).WithMessage("Severity must be Minor, Major or Critical");

        }   
    }
}

