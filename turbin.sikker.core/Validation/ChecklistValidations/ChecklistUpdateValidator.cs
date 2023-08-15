﻿using FluentValidation;
using turbin.sikker.core.Model.DTO.ChecklistDtos;

namespace turbin.sikker.core.Validation.ChecklistValidations
{
    public class ChecklistUpdateValidator : AbstractValidator<ChecklistEditDto>
    {
        public ChecklistUpdateValidator()
        {
            RuleFor(checklist => checklist.Title)
                .MinimumLength(3).WithMessage("Checklist title must be at least 3 characters.")
                .MaximumLength(20).WithMessage("Checklist title cannot exceed 20 characters.")
                .Matches("^[a-zæøåA-ZÆØÅ0-9_.\\- ]+$").WithMessage("Category name can only contain letters, numbers, underscores. periods or hyphens.");
        }
    }
}
