using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTrackingSystem.Data.Models
{
    public class Invitation
    {
        [Key]
        public int Id { get; set; }
        public string InvitationSenderUserId { get; set; } = string.Empty;
        [ForeignKey("InvitationSenderUserId")]
        public ApplicationUser? ApplicationUserSender { get; set; }
        public string InvitationReceiverUserId { get; set; } = string.Empty;
        [ForeignKey("InvitationReceiverUserId")]
        public ApplicationUser? ApplicationUserReceiver { get; set; }
       public enum Status
        {
            Approved=1,
            Reject=2,
            Pending=3
        };
        public Status InvitedPersonStatus { get; set; }
        public enum Action
        {
            Enable=1,
            Disable=2,
            Deleted=3
        };
        public Action InvitedPersonAction { get; set; }

    }
}
