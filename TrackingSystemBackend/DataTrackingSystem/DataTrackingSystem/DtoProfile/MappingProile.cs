using AutoMapper;
using DataTrackingSystem.Data.Models;
using DataTrackingSystem.Data.ViewModel.Input;

namespace DataTrackingSystem.DtoProfile
{
    public class MappingProile:Profile
    {
        public MappingProile()
        {
            CreateMap<ShippingViewModel, Shipping>().ReverseMap();
        }
    }
}
