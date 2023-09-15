using FluentValidation;
using turbin.sikker.core.Model.DTO.TaskDtos;

namespace turbin.sikker.core.Validation.ChecklistTaskValidations
{
    public class ChecklistTaskUpdateValidation : AbstractValidator<ChecklistTaskUpdateDto>
    {
        public ChecklistTaskUpdateValidation()
        {
            RuleFor(task => task.Description).NotNull().NotEmpty().WithMessage("Description is required.")
                .MinimumLength(5).WithMessage("Description must be at least 5 characters.");
            
             RuleFor(task => task.CategoryId).NotNull().NotEmpty().WithMessage("Category ID is required.");
        }
    }
}

