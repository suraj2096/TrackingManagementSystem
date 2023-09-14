using DataTrackingSystem.Data.ViewModel.Input;
using FluentValidation;

namespace DataTrackingSystem.Validators
{
    public class RegisterValidator:AbstractValidator<Register>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().NotNull().WithMessage("Enter the userName");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Enter the email properly");
            RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Enter the password");
        }
    }
}
