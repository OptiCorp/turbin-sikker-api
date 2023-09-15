using FluentValidation;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Validation.ChecklistTaskValidations
{
    public class PunchCreateValidation : AbstractValidator<PunchCreateDto>
    {
        public PunchCreateValidation()
        {
            RuleFor(punch => punch.Severity).Must((severity) => severity == "Minor" || severity == "Major" || severity == "Critical" || severity == null).WithMessage("Severity must be Minor, Major or Critical");

        }
    }
}

