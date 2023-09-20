using FluentValidation;
using turbin.sikker.core.Model.DTO.TaskDtos;

namespace turbin.sikker.core.Validation.ChecklistTaskValidations
{
    public class ChecklistTaskCreateValidation : AbstractValidator<ChecklistTaskCreateDto>
    {
        public ChecklistTaskCreateValidation()
        {
            RuleFor(task => task.Description).NotEmpty().WithMessage("Description is required.")
                .NotNull().WithMessage("Description cannot be null.")
                .MinimumLength(5).WithMessage("Description must be at least 5 characters.")
                .MaximumLength(500).WithMessage("Category name cannot exceed 500 characters.");
            
             RuleFor(task => task.CategoryId).NotEmpty().WithMessage("Category ID is required.")
                .NotNull().WithMessage("Category ID cannot be null.");
        }
    }
}

