using FluentValidation;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Validation.ChecklistTaskValidations
{
    public class PunchUpdateValidation : AbstractValidator<PunchUpdateDto>
    {
        public PunchUpdateValidation()
        {   
            RuleFor(punch => punch.Id).NotEmpty().WithMessage("You must specify a ID for this punch").NotNull().WithMessage("You must specify a ID for this punch");  

            RuleFor(punch => punch.Severity).Must((severity) => severity == "Minor" || severity == "Major" || severity == "Critical" || severity == null).WithMessage("Severity must be Minor, Major or Critical");

            RuleFor(punch => punch.Status).Must((status) => status == "Pending" || status == "Approved" || status == "Rejected" || status == null).WithMessage("Status must be Pending, Approved or Rejected");
        }
    }
}

