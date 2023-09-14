using DataTrackingSystem.Data.Models;
using DataTrackingSystem.Data.ViewModel.Input;
using DataTrackingSystem.Data.ViewModel.Output;
using DataTrackingSystem.Service.IService;
using DataTrackingSystem.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataTrackingSystem.Service
{
    public class LoginRegisterService : ILoginRegisterService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly LoginValidator _loginValidator;
        private readonly RegisterValidator _registerValidator;
        public LoginRegisterService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,ITokenHandler tokenHandler, LoginValidator loginValidator, RegisterValidator registerValidator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        #region Authenticate the user in a database........
        public async  Task<LoginOutput?> AuthenticateUser(Login login)
        {
            var loginDataValidate = _loginValidator.Validate(login);
            if (!loginDataValidate.IsValid)
            {
                return new LoginOutput()
                {
                    Status = false,
                    Message = JsonConvert.SerializeObject(loginDataValidate.Errors.ToList().Select(u=>u.ErrorMessage))
                };
            }
           
                // here we first check user exist in the database or not if not then not go user further.
                var userExist = await _userManager.FindByNameAsync(login.UserName??"");
                if (userExist == null)
                return new LoginOutput()
                {
                    Status = false,
                    Message = "Go to the register first then login"
                };
                
                // here we verify that user is genuine or not 
                var userVerification = await _signInManager.CheckPasswordSignInAsync(userExist, login.Password, false);
                if (!userVerification.Succeeded)
                return new LoginOutput()
                {
                    Status = false,
                    Message = "You enter the wrong password"
                };
                // here user will register successfully to the database and we will return the login output to the user.
            return new LoginOutput()
            {
                Status = true,
                Message = "User Login Successfully!!!!",
                Token = _tokenHandler.GenerateToken(userExist.Id)
            };

  
            
        }
        #endregion

        #region Register the user here.
        public async Task<bool> RegisterUser(Register register)
        {
            var registerDataValidate = _registerValidator.Validate(register);
            if (!registerDataValidate.IsValid)
            {
                return false;
            }

            // register the user
            ApplicationUser applicationUser = new ApplicationUser()
            {
                UserName = register.UserName,
                Email = register.Email,
                PasswordHash = register.Password
            };
            var user = await _userManager.CreateAsync(applicationUser, applicationUser.PasswordHash);
            if (!user.Succeeded) return false;
            return true;
        }
        #endregion
    }
}
