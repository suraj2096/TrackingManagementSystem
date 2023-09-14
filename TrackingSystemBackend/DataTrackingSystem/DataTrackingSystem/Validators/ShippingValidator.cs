using DataTrackingSystem.Data.Models;
using DataTrackingSystem.Data.ViewModel.Input;
using FluentValidation;

namespace DataTrackingSystem.Validators
{
    public class ShippingValidator:AbstractValidator<ShippingViewModel>
    {
        public ShippingValidator() {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Shipping Name is required");
            RuleFor(x => x.SenderAddress).NotEmpty().WithMessage("Sender Message is required");
            RuleFor(x => x.ReceiverAddress).NotEmpty().WithMessage("Receiver Message is required");
           // RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
            RuleFor(x => x.ShippingMethod).NotEmpty().WithMessage("Shipping Method is required");
            RuleFor(x => x.ShippingStatus).NotEmpty().WithMessage("Shipping Status is required");
        }  
    }
}
