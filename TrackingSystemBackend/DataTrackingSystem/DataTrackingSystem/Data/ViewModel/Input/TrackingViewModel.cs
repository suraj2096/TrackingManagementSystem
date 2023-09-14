using System.Text.Json.Serialization;

namespace DataTrackingSystem.Data.ViewModel.Input
{
    public class TrackingViewModel
    {

        public string TrackingId { get; set; } = Guid.NewGuid().ToString();
       // [JsonPropertyName("shippingId")]
        public int ShippingId { get; set; } = 0;
        public string UserId { get; set; } = string.Empty;

       // [JsonPropertyName("trackingDate")]
        public DateTime TrackingDate { get; set; }
    }
}
