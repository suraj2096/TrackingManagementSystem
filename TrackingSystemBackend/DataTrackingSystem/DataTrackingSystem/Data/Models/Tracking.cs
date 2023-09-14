using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTrackingSystem.Data.Models
{
    public class Tracking
    {
        [Key]
        public string TrackingId { get; set; } = Guid.NewGuid().ToString();
        public int ShippingId { get; set; } = 0;
        public Shipping? Shipping { get; set; }
        public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))]
        public ApplicationUser? ApplicationUser { get; set; }

        public string? DataChangeUserId { get; set; }
        [ForeignKey(nameof(DataChangeUserId))]
        public ApplicationUser? ApplicationUserDataChange { get; set; }
        public DateTime TrackingDate { get; set; }
        public enum Operation
        {
            Add=1,
            Update=2,
            Delete=3
        };
        public Operation UserOperation { get; set; }
        

    }
}
