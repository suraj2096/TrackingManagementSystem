using DataTrackingSystem.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataTrackingSystem.Data.ViewModel.Output
{
    public class TrackingOutput
    {
        public string TrackingId { get; set; }
      
        public int ShippingId { get; set; } = 0;
        public Shipping? Shipping { get; set; }
        public DateTime TrackingDate { get; set; }
        public enum Operation
        {
            Add = 1,
            Update = 2,
            Delete = 3
        };
        public Operation UserOperation { get; set; }
        public string? DataChangeUserId { get; set; }
        [ForeignKey(nameof(DataChangeUserId))]
        public ApplicationUser? ApplicationUserDataChange { get; set; }
    }
}
