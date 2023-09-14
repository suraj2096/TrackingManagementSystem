using DataTrackingSystem.Data.Models;
using DataTrackingSystem.Data.ViewModel.Output;
using Microsoft.AspNetCore.Identity;

namespace DataTrackingSystem;
public class UnitOfWork
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UnitOfWork(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public ApplicationUser? CheckPersonsId(string userId)
    {
        var validatasender = _userManager.FindByIdAsync(userId).Result;
        return validatasender;
    }
}
    

