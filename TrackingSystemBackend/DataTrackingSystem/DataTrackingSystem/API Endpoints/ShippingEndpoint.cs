using AutoMapper;
using DataTrackingSystem.Data.Models;
using DataTrackingSystem.Data.ViewModel.Input;
using DataTrackingSystem.Data.ViewModel.Output;
using DataTrackingSystem.Repository;
using DataTrackingSystem.Repository.IRepository;
using DataTrackingSystem.Service.IService;
using DataTrackingSystem.Validators;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataTrackingSystem.API_Endpoints
{
    public static class ShippingEndpoint
    {
       
        public static RouteGroupBuilder ShippingApi(this RouteGroupBuilder builder)
        {
            builder.MapGet("/shippings",GetAllShipping );
            builder.MapGet("/shipping/{shippingId}", GetShipping);
            builder.MapPost("/createShipping", CreateShipping);
            builder.MapPut("/updateShipping", UpdateShipping); 
            builder.MapDelete("/deleteShipping/{shippingId}", DeleteShipping);
            return builder;
        }
        /// <summary>
        /// Here in this function we will get all the shipping list.
        /// </summary>
        /// <param name="shippingRepository"></param>
        /// <returns></returns>
        public static IResult GetAllShipping(IShippingRepository shippingRepository, 
            ITokenHandler tokenHandler, IHttpContextAccessor _httpContextAccessor, ITrackingRepository _trackingRepository,
             UnitOfWork _unitofWork, IMapper _mapper)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var getSenderId = tokenHandler.GetUserIdFromToken(token);
            if (getSenderId == null) return Results.BadRequest();
            var data = shippingRepository.GetShippings(getSenderId);
            // now i will do mapping ......................
            IList<ShippingViewModel> shipping = _mapper.Map<IList<ShippingViewModel>>(data);
            if (data.Count== 0) return Results.Ok(data);
            var findTracking = _trackingRepository.getAllTracking(data.FirstOrDefault().UserId);
            foreach(var tracking in findTracking) {
                shipping.FirstOrDefault(u => u.ShippingId == tracking.ShippingId).TrackingDetails.Add(
                    new TrackingOutput()
                    {
                        TrackingId = tracking.TrackingId,   
                        ShippingId= tracking.ShippingId,
                        DataChangeUserId= tracking.TrackingId,
                        ApplicationUserDataChange= _unitofWork.CheckPersonsId(tracking.UserId),
                        UserOperation=(TrackingOutput.Operation)tracking.UserOperation,
                        TrackingDate= tracking.TrackingDate
                    });
            }
            return Results.Ok(shipping);

        }
        /// <summary>
        /// herew we will get a particular shippping 
        /// </summary>
        /// <param name="shippingId"></param>
        /// <param name="shippingRepository"></param>
        /// <returns></returns>
        public static IResult GetShipping(int shippingId, IShippingRepository shippingRepository)
        {
            if (shippingId == 0) return Results.BadRequest();
            return Results.Ok(shippingRepository.GetShipping(shippingId));
        }
        /// <summary>
        /// Here we will Create a shipping 
        /// </summary>
        /// <param name="shipping"></param>
        /// <param name="shippingValidator"></param>
        /// <param name="shippingRepository"></param>
        /// <returns></returns>
        public static IResult CreateShipping(ShippingViewModel shippingView,
            ShippingValidator shippingValidator,IShippingRepository shippingRepository,
            IMapper _mapper, ITokenHandler tokenHandler, IHttpContextAccessor 
            _httpContextAccessor,
            UnitOfWork _unitofWork,
            ITrackingRepository _trackingRepository)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var getSenderId = tokenHandler.GetUserIdFromToken(token);
            if (getSenderId == null) return Results.BadRequest();
            var shippingValidate = shippingValidator.Validate(shippingView);
            if (!shippingValidate.IsValid)
            {
                return Results.Ok(new { Status = 0, Message = shippingValidate.Errors.ToList().Select(x => x.ErrorMessage).ToString() });
            }
            // befor going further first we will check the ids or person in database or not
            var checkSenderInDatabase = _unitofWork.CheckPersonsId(getSenderId);
             if(checkSenderInDatabase== null) return Results.BadRequest();  
            // we will check the userid if user send invitation to other person to crete its data so thats why i will take that decision.
            if(shippingView.UserId != "")
            {
                var data = _unitofWork.CheckPersonsId(shippingView.UserId);
                if(data == null) return Results.BadRequest();
                shippingView.UserId = data.Id;
            }
            else
            {
                shippingView.UserId = checkSenderInDatabase.Id;
            }
            Shipping shipping = _mapper.Map<Shipping>(shippingView);
            //shipping.ApplicationUser = null;
            var addShipping = shippingRepository.CreateShipping(shipping);
            if (!addShipping) {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
              }
            // now we will create a tracking here .......................
            // we will check first tracking start or not ........................
         
            if (shippingView.UserId != getSenderId)
            {
                // here we will perform tracking.................
                Tracking tracking = new Tracking()
                {
                    ShippingId = shipping.ShippingId,
                    UserId = getSenderId,
                    TrackingDate = DateTime.UtcNow,
                    UserOperation = Tracking.Operation.Add,
                    DataChangeUserId= shipping.UserId
                };

                var trackingCreate = _trackingRepository.CreateTracking(tracking);

                if(!trackingCreate) { return Results.StatusCode(StatusCodes.Status500InternalServerError); }
                shippingView.TrackingDetails.Add(
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

                return Results.Ok(new { Status = 1, Message = "Shipping created successfully", data = shipping });
        }
        /// <summary>
        /// Here we will update the shipping hte detail.
        /// </summary>
        /// <param name="shipping"></param>
        /// <param name="shippingValidator"></param>
        /// <param name="shippingRepository"></param>
        /// <returns></returns>
        public static IResult UpdateShipping([FromBody]ShippingViewModel shippingView, ShippingValidator shippingValidator,
            IShippingRepository shippingRepository, IMapper _mapper, ITokenHandler tokenHandler, IHttpContextAccessor
            _httpContextAccessor, UnitOfWork _unitofWork, ITrackingRepository _trackingRepository)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var getSenderId = tokenHandler.GetUserIdFromToken(token);
            if (getSenderId == null) return Results.BadRequest();
            var shippingValidate = shippingValidator.Validate(shippingView);
            if (!shippingValidate.IsValid)
            {
                return Results.Ok(new { Status = 0, Message = shippingValidate.Errors.ToList().Select(x => x.ErrorMessage).ToString() });
            }
            // befor going further first we will check the ids or person in database or not
            var checkSenderInDatabase = _unitofWork.CheckPersonsId(getSenderId);
            if (checkSenderInDatabase == null) return Results.BadRequest();
            // we will check the userid if user send invitation to other person to crete its data so thats why i will take that decision.
            if (shippingView.UserId != "")
            {
                var data = _unitofWork.CheckPersonsId(shippingView.UserId);
                if (data == null) return Results.BadRequest();
                shippingView.UserId = data.Id;
            }
            else
            {
                shippingView.UserId = checkSenderInDatabase.Id;
            }
            Shipping shipping = _mapper.Map<Shipping>(shippingView);
            //shipping.ApplicationUser = null;
            var updateShipping = shippingRepository.UpdateShipping(shipping);
            if (!updateShipping)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
            // now we will create a tracking here .......................
            // we will check first tracking start or not ........................

            if (shippingView.UserId != getSenderId)
            {
                // here we will perform tracking.................
                Tracking tracking = new Tracking()
                {
                    ShippingId = shipping.ShippingId,
                    UserId = getSenderId,
                    TrackingDate = DateTime.UtcNow,
                    UserOperation = Tracking.Operation.Update,
                    DataChangeUserId = shipping.UserId
                };

                var trackingCreate = _trackingRepository.CreateTracking(tracking);

                if (!trackingCreate) { return Results.StatusCode(StatusCodes.Status500InternalServerError); }
                shippingView.TrackingDetails.Add(
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

            return Results.Ok(new { Status = 1, Message = "Shipping created successfully", data = shipping });






        }
        /// <summary>
        /// Here we will delete the shipping .
        /// </summary>
        /// <param name="shippingId"></param>
        /// <param name="shippingRepository"></param>
        /// <returns></returns>
        public static IResult DeleteShipping(int shippingId, IShippingRepository shippingRepository, IMapper _mapper, ITokenHandler tokenHandler, IHttpContextAccessor
            _httpContextAccessor, UnitOfWork _unitofWork, ITrackingRepository _trackingRepository)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var getSenderId = tokenHandler.GetUserIdFromToken(token);
            if (getSenderId == null) return Results.BadRequest();
            
            // befor going further first we will check the ids or person in database or not
            var checkSenderInDatabase = _unitofWork.CheckPersonsId(getSenderId);
            if (checkSenderInDatabase == null) return Results.BadRequest();
            // we will check the userid if user send invitation to other person to crete its data so thats why i will take that decision.
            if (shippingId == 0) return Results.BadRequest();
            var getShipping = shippingRepository.GetShipping(shippingId);
            if (getShipping.UserId != checkSenderInDatabase.Id)
            {
                var data = _unitofWork.CheckPersonsId(checkSenderInDatabase.Id);
                if (data == null) return Results.BadRequest();
                // here we will perform tracking.................
                Tracking tracking = new Tracking()
                {
                    ShippingId = getShipping.ShippingId,
                    UserId = getSenderId,
                    TrackingDate = DateTime.UtcNow,
                    UserOperation = Tracking.Operation.Delete,
                    DataChangeUserId = getShipping.UserId
                };
                if (!_trackingRepository.CreateTracking(tracking)) { Results.StatusCode(StatusCodes.Status500InternalServerError); }
                getShipping.isDeleted = true;
                 shippingRepository.UpdateShipping(getShipping);
                return Results.Ok(new { Status = 1, Message = "Shipping deleted successfully", data = getShipping });
            }

            var deleteShipping = shippingRepository.DeleteShipping(shippingId);
            if (deleteShipping) return Results.Ok(new { Status = 1, Message = "Deleted Successfully!!" });
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
