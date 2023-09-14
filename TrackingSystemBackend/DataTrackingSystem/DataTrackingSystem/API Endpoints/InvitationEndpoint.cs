using AutoMapper;
using Azure.Core;
using DataTrackingSystem.Data.ViewModel.Input;
using DataTrackingSystem.Data.ViewModel.Output;
using DataTrackingSystem.Repository;
using DataTrackingSystem.Repository.IRepository;
using DataTrackingSystem.Service;
using DataTrackingSystem.Service.IService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Dynamic;
using static DataTrackingSystem.Data.Models.Invitation;

namespace DataTrackingSystem.API_Endpoints
{
    public static class InvitationEndpoint
    {
        public static RouteGroupBuilder InvitationEndpointApi(this RouteGroupBuilder builder)
        {
            builder.MapGet("/findPerson/{personName}", FindPerson);
            builder.MapPost("/invitationCreator", InvitationCreator);
            builder.MapGet("/getInvitedPerson", GetInvitedPerson);
            builder.MapGet("/changeInvitationTable/{receiverId}/{status:int}", ChangeInvitaionStatus);
            builder.MapGet("/changeInvitationTableAction/{receiverId}/{action:int}", TakeActionOnInvitation);
            builder.MapGet("getInvitationComesFromUser", InvitationComesFromUsers);
            builder.MapGet("getInvitationerShipping/{invitaionerId}", GetInvitationerShipping);
            return builder;
        }
        /// <summary>
        /// Here in this function we will find the user that sender user will search in the textbox.
        /// </summary>
        /// <param name="personName"></param>
        /// <param name="invitationRepository"></param>
        /// <param name="tokenHandler"></param>
        /// <param name="_httpContextAccessor"></param>
        /// <returns></returns>
        public static IResult FindPerson(string personName, IInvitaionRepository invitationRepository, ITokenHandler tokenHandler, IHttpContextAccessor _httpContextAccessor)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var getSenderId = tokenHandler.GetUserIdFromToken(token);
            if (getSenderId == null) return Results.BadRequest();
            var data = invitationRepository.FindPersons(personName,getSenderId);
            return Results.Ok(data);
        }
        
        
        /// <summary>
        /// Here we will create the invitation and send the email to the invited person to approve or reject the invitation.
        /// </summary>
        /// <param name="invitationer"></param>
        /// <param name="invitationRepository"></param>
        /// <param name="tokenHandler"></param>
        /// <param name="_httpContextAccessor"></param>
        /// <returns></returns>
        public static IResult InvitationCreator([FromBody]FindPerson invitationer, IInvitaionRepository invitationRepository,  ITokenHandler tokenHandler,IHttpContextAccessor _httpContextAccessor)
        {
            if (string.IsNullOrEmpty(invitationer.Id))
                return Results.BadRequest();
            
            // here we will get the token form httpcontext ........
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var getSenderId = tokenHandler.GetUserIdFromToken(token);



           




            if(getSenderId == null) 
                return Results.BadRequest(new {message="your token doesnot contain user id "}) ;


            var result = invitationRepository.CreateInvitation(getSenderId, invitationer.Id);
            return Results.Ok(result);
        }
       
        
        
        /// <summary>
        /// Here in this function we will get the specific person that invited the other user.
        /// </summary>
        /// <returns></returns>
        
        public static IResult GetInvitedPerson(IInvitaionRepository invitationRepository, ITokenHandler tokenHandler, IHttpContextAccessor _httpContextAccessor)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var getSenderId = tokenHandler.GetUserIdFromToken(token);
            if (getSenderId == null) return Results.BadRequest();
            var data = invitationRepository.InvitedPersonList(getSenderId);
            return Results.Ok(data);
        }
        /// <summary>
        /// Here we will change the invitaion staus that will be send to the receiver....
        /// </summary>
        /// <param name="invitationRepository"></param>
        /// <param name="tokenHandler"></param>
        /// <param name="_httpContextAccessor"></param>
        /// <returns></returns>
        public static IResult ChangeInvitaionStatus(string receiverId,int status,IInvitaionRepository invitationRepository, ITokenHandler tokenHandler, IHttpContextAccessor _httpContextAccessor)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var getSenderId = tokenHandler.GetUserIdFromToken(token);
            if (getSenderId == null || receiverId == null || status == 0) return Results.BadRequest();

            if(!invitationRepository.ChangeInvitationStatus(receiverId, getSenderId, status))
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Results.Ok(new{Status=1,Message="Invitation Updated Successfully" });
        }
        /// <summary>
        /// Here we will take action on the invited person means enable,disable and deleted the user..
        /// </summary>
        /// <param name="receiverId"></param>
        /// <param name="action"></param>
        /// <param name="invitationRepository"></param>
        /// <param name="tokenHandler"></param>
        /// <param name="_httpContextAccessor"></param>
        /// <returns></returns>
        public static IResult TakeActionOnInvitation(string receiverId,int action, IInvitaionRepository invitationRepository, ITokenHandler tokenHandler, IHttpContextAccessor _httpContextAccessor)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var getSenderId = tokenHandler.GetUserIdFromToken(token);
            if (getSenderId == null || receiverId == null || action == 0) return Results.BadRequest();

            if (!invitationRepository.TakeActionOnInvitedPerson(receiverId, getSenderId, action))
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Results.Ok(new { Status = 1, Message = "Invitation Updated Successfully!!!!!" });
        }

        public static IResult InvitationComesFromUsers(IInvitaionRepository invitationRepository, ITokenHandler tokenHandler, IHttpContextAccessor _httpContextAccessor)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = tokenHandler.GetUserIdFromToken(token);
            if (userId == null) return Results.BadRequest();

            var data = invitationRepository.InvitationComesFromUser(userId);
            return Results.Ok(data);
        }


        // get specific invitaioner shipping......
        public static IResult GetInvitationerShipping(string? invitaionerId, IShippingRepository shippingRepository, ITokenHandler tokenHandler, IHttpContextAccessor _httpContextAccessor,
            ITrackingRepository _trackingRepository,
            IMapper _mapper,
            UnitOfWork _unitofWork)
        {
            if (string.IsNullOrEmpty(invitaionerId))
                return Results.BadRequest();
            var data = shippingRepository.GetShippings(invitaionerId);
            // now i will do mapping ......................
            IList<ShippingViewModel> shipping = _mapper.Map<IList<ShippingViewModel>>(data);
            if (data.Count == 0) return Results.Ok(data);
            var findTracking = _trackingRepository.getAllTracking(data.FirstOrDefault().UserId);
            foreach (var tracking in findTracking)
            {
                shipping.FirstOrDefault(u => u.ShippingId == tracking.ShippingId).TrackingDetails.Add(
                    new TrackingOutput()
                    {
                        TrackingId = tracking.TrackingId,
                        ShippingId = tracking.ShippingId,
                        DataChangeUserId = tracking.TrackingId,
                        ApplicationUserDataChange = _unitofWork.CheckPersonsId(tracking.UserId),
                        UserOperation = (TrackingOutput.Operation)tracking.UserOperation,
                        TrackingDate = tracking.TrackingDate
                    });
            }
            return Results.Ok(shipping);
        }
    }
}
