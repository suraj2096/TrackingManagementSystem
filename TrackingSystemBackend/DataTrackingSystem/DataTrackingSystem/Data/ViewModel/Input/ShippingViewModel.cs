using DataTrackingSystem.Data.Models;
using DataTrackingSystem.Data.ViewModel.Output;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataTrackingSystem.Data.ViewModel.Input
{
    public class ShippingViewModel
    {
        public ShippingViewModel() {
            TrackingDetails = new List<TrackingOutput>();
        }
        [JsonPropertyName("shippingId")]
        public int ShippingId { get; set; }
       [JsonPropertyName("name")]
       
        public string Name { get; set; } = string.Empty;
      [JsonPropertyName("senderAddress")]
       
        public string SenderAddress { get; set; } = string.Empty;
       [JsonPropertyName("receiverAddress")]
      
        public string ReceiverAddress { get; set; } = string.Empty;
      [JsonPropertyName("shippingMethod")]
       
        public string ShippingMethod { get; set; } = string.Empty;
       [JsonPropertyName("shippingStatus")]
        
        public string ShippingStatus { get; set; } = string.Empty;
        [JsonPropertyName("shippingCost")]
        public int ShippingCost { get; set; } = 0;
        [JsonPropertyName("UserId")]
        public string? UserId { get; set; } = string.Empty;
        public ApplicationUser? ApplicationUser { get; set; }
        //[JsonPropertyName("IsDeleted")]
        public bool? isDeleted { get; set; } = false;

        public IList<TrackingOutput> TrackingDetails { get; set; }

    }
}
