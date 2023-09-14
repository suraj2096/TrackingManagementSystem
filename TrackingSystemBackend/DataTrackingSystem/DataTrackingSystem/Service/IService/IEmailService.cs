using DataTrackingSystem.Data.ViewModel.Input;

namespace DataTrackingSystem.Service.IService
{
    public interface IEmailService
    {
        void SendEmail(MailRequest message);
    }
}
