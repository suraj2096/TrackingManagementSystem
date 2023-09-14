using DataTrackingSystem.Data.ViewModel.Input;
using DataTrackingSystem.Data.ViewModel.Output;
using DataTrackingSystem.Service;
using DataTrackingSystem.Service.IService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.CompilerServices;

namespace DataTrackingSystem.API_Endpoints
{
    public static class LoginEndpoint
    {
      
        public static RouteGroupBuilder LoginRegisterApi(this RouteGroupBuilder builder)
        {
            builder.MapPost("/login", LoginUser);
            builder.MapPost("/register", RegisterUser);
            return builder;
        }
        /// <summary>
        /// this is the LoginUser function that will be called at the api/login endpoint
        /// </summary>
        /// <param name="login"></param>
        /// <param name="loginRegisterService"></param>
        /// <returns></returns>
        public async static Task<IResult> LoginUser([FromBody] Login login,ILoginRegisterService loginRegisterService)
        {
            var loginResult = await loginRegisterService.AuthenticateUser(login);
            if (!loginResult.Status)
            {
                return Results.BadRequest(loginResult);
            }
            return Results.Ok(loginResult);
        }
       /// <summary>
       /// this is the RegisterUser function that will be called at the api/register 
       /// </summary>
       /// <param name="register"></param>
       /// <param name="loginRegisterService"></param>
       /// <returns></returns>
        public async static Task<IResult>  RegisterUser([FromBody] Register register, ILoginRegisterService loginRegisterService)
        {
            var registerSuccess = await loginRegisterService.RegisterUser(register);
            if (!registerSuccess)
            {
                return Results.BadRequest(new {Status=0, Message = "Something went wrong" });
            }
            return Results.Ok(new {Status=1, Message = "Register Successfully!!" });
        }

    }
}
