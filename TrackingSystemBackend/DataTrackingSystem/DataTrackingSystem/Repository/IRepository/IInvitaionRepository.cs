using DataTrackingSystem.Data.Models;
using DataTrackingSystem.Data.ViewModel.Output;

namespace DataTrackingSystem.Repository.IRepository
{
    public interface IInvitaionRepository
    {
        public ICollection<FindPerson> FindPersons(string personName,string senderInvitationId);
        public bool CreateInvitation(string senderInvitationId, string ReceiverInvitationId);
        public bool TakeActionOnInvitedPerson(string receiverId, string senderId, int action);
        public ICollection<Invitation> InvitedPersonList(string senderId);
        public ICollection<Invitation> InvitationComesFromUser(string userId);
        public bool ChangeInvitationStatus(string receiverId, string senderId, int status);
    }
}
