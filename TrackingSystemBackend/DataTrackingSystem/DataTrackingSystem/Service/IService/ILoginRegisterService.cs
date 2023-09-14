using DataTrackingSystem.Data.Models;
using DataTrackingSystem.Data.ViewModel.Input;
using DataTrackingSystem.Data.ViewModel.Output;

namespace DataTrackingSystem.Service.IService
{
    public interface ILoginRegisterService
    {
        Task<LoginOutput?> AuthenticateUser(Login login);
        Task<bool> RegisterUser(Register register);
    }
}
