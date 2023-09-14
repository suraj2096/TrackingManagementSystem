using DataTrackingSystem.Data.ViewModel.Input;
using FluentValidation;

namespace DataTrackingSystem.Validators;

public class LoginValidator:AbstractValidator<Login>
{
    public LoginValidator()
    {
        RuleFor(x=>x.UserName).NotEmpty().NotNull().WithMessage("Enter the UserName");  
        RuleFor(x=>x.Password).NotEmpty().NotNull().WithMessage("Enter the Password");


    }
}
