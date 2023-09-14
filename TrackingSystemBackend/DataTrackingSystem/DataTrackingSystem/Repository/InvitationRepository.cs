using DataTrackingSystem.Data.Models;
using DataTrackingSystem.Data.ViewModel.Input;
using DataTrackingSystem.Data.ViewModel.Output;
using DataTrackingSystem.Repository.IRepository;
using DataTrackingSystem.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using static DataTrackingSystem.Data.Models.Invitation;

namespace DataTrackingSystem.Repository
{
    public class InvitationRepository : IInvitaionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _email;
      


        public InvitationRepository(ApplicationDbContext cotext,UserManager<ApplicationUser> userManager, IEmailService email)
        {
             _context= cotext;
            _userManager= userManager;
            _email= email;
        }
      
        public ICollection<FindPerson> FindPersons(string personName,string senderInvitationId)
        {
            var validatasender = _userManager.FindByIdAsync(senderInvitationId).Result;
            var personMatchList = _userManager.Users;
            return personMatchList.Where(u => u.UserName.Contains(personName)).Select(m => new FindPerson() { Id = m.Id, Name = m.UserName }).Where(u=>u.Id !=validatasender.Id).ToList();

        }
        public bool CreateInvitation(string senderInvitationId,string ReceiverInvitationId)
        {
            
            var validatasender = _userManager.FindByIdAsync(senderInvitationId).Result;
            var validateReceiver = _userManager.FindByIdAsync(ReceiverInvitationId).Result;
            if (validateReceiver == null || validatasender == null)
                return false;
            if(_context.Invitations.FirstOrDefault(a=>a.InvitationReceiverUserId == validateReceiver.Id)!= null)
            {
                return false;
            }
            // now we will create invitation here ...............................
            Invitation invitation = new Invitation()
            {
                InvitationReceiverUserId = validateReceiver.Id,
                InvitationSenderUserId = validatasender.Id,
                InvitedPersonStatus = Invitation.Status.Pending,
                InvitedPersonAction = Invitation.Action.Disable
            };
            _context.Invitations.Add( invitation );
            var saveInvitation= _context.SaveChanges()==1?true:false;
            if (!saveInvitation)
                return false;
            var mailrequest = new MailRequest()
            {
                FromEmail = validatasender.Email,
                ToEmail = "suraj-gusain@cssoftsolutions.com", // validateReceiver.Email
                receiverUserName = validateReceiver.UserName,
                senderUserName = validatasender.UserName,
                ReceiverId = validateReceiver.Id
            };
            _email.SendEmail(mailrequest);
            return true;
        }

      

        public ICollection<Invitation> InvitationComesFromUser(string userId)
        {
            return _context.Set<Invitation>().Include(u => u.ApplicationUserSender)
                .Where(u => u.InvitationReceiverUserId == userId && u.InvitedPersonStatus == Invitation.Status.Approved)
                .Select(u => new Invitation()
                {
                    InvitationSenderUserId = u.InvitationSenderUserId,
                    ApplicationUserSender = new ApplicationUser()
                    {
                        Id = u.ApplicationUserSender.Id,
                        UserName = u.ApplicationUserSender.UserName
                    },
                    InvitedPersonAction = u.InvitedPersonAction,
                    InvitedPersonStatus = u.InvitedPersonStatus
                }
                )
                .ToList();
        }

        public ICollection<Invitation> InvitedPersonList(string senderId)
        {
            // here we will use set to get records from multiple tables concurrently ...............
            // we use include to chaining of query.
            return _context.Set<Invitation>().Include(u => u.ApplicationUserReceiver)
                .Where(u => u.InvitationSenderUserId == senderId)
                .Select(u => new Invitation()
                {
                    InvitationReceiverUserId = u.InvitationReceiverUserId,
                    InvitationSenderUserId = u.InvitationSenderUserId,
                    ApplicationUserReceiver = new ApplicationUser()
                    {
                        UserName = u.ApplicationUserReceiver.UserName 
                    },
                    InvitedPersonAction= u.InvitedPersonAction,
                    InvitedPersonStatus= u.InvitedPersonStatus
                }
                )
                .ToList();   
        }

        public bool TakeActionOnInvitedPerson(string receiverId,string senderId,int action)
        {
          if(action>4)
            {
                return false;
            }
            var validatasender = _userManager.FindByIdAsync(senderId).Result;
            var validateReceiver = _userManager.FindByIdAsync(receiverId).Result;
            if (validateReceiver == null || validatasender == null)
                return false;
            var findInvitation = _context.Invitations.FirstOrDefault(m => m.InvitationSenderUserId == senderId && m.InvitationReceiverUserId == receiverId);
          
                if (findInvitation == null) return false;
            findInvitation.InvitedPersonAction = (Invitation.Action)action;
            if(findInvitation.InvitedPersonAction == Invitation.Action.Deleted)
            {
                _context.Invitations.Remove(findInvitation);
                return _context.SaveChanges() == 1 ? true : false;
            }
            // here we will update the database .......
            _context.Invitations.Update(findInvitation);
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool ChangeInvitationStatus(string receiverId,string senderId,int status)
        {
            var validatasender = _userManager.FindByIdAsync(senderId).Result;
            var validateReceiver = _userManager.FindByIdAsync(receiverId).Result;
            if (validateReceiver == null || validatasender == null)
                return false;
            var findInvitation = _context.Invitations.FirstOrDefault(m => m.InvitationSenderUserId == senderId && m.InvitationReceiverUserId == receiverId);
            if(findInvitation == null) return false;
            findInvitation.InvitedPersonStatus = (Invitation.Status) status;
            if (findInvitation.InvitedPersonStatus == Invitation.Status.Approved)
                findInvitation.InvitedPersonAction = Invitation.Action.Enable;

            // here we will update the database .......
            _context.Update(findInvitation);    
            return _context.SaveChanges()==1?true:false ;
        }
    }
}
