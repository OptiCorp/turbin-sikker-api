﻿using FluentValidation;
using turbin.sikker.core.Model.DTO.CategoryDtos;

namespace turbin.sikker.core.Validation.CategoryValidations
{
    public class CategoryCreateValidator : AbstractValidator<CategoryCreateDto>
    {

        public CategoryCreateValidator()
        {
            RuleFor(category => category.Name).NotEmpty().WithMessage("Category name is required.")
                .NotNull().WithMessage("Category name cannot be null.")
                .MinimumLength(3).WithMessage("Category name must be at least 3 characters.")
                .MaximumLength(20).WithMessage("Category name cannot exceed 20 characters.")
                .Matches("^[a-zA-Z0-9_.\\- ]+$").WithMessage("Category name can only contain letters, numbers, underscores, periods or hyphens.");
        }
    }
}

