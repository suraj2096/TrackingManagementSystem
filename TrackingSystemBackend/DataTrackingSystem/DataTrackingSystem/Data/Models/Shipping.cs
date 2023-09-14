using DataTrackingSystem.Data.ViewModel.Input;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTrackingSystem.Data.Models
{
    public class Shipping
    {
        [Key]
        public int ShippingId { get; set; }
       
        public string Name { get; set; } = string.Empty;
        
        public string SenderAddress { get; set; } = string.Empty;
       
        public string ReceiverAddress { get; set; } = string.Empty;
       
        public string ShippingMethod { get; set; } = string.Empty;
      
        public string ShippingStatus { get; set; } = string.Empty;
        public int ShippingCost { get; set; } = 0;
       
        public string? UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))]
        public ApplicationUser? ApplicationUser { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}
