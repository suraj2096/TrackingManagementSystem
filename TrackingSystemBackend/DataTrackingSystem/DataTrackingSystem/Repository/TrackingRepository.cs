using DataTrackingSystem.Data.Models;
using DataTrackingSystem.Repository.IRepository;

namespace DataTrackingSystem.Repository
{
    public class TrackingRepository : ITrackingRepository
    {
        private readonly ApplicationDbContext _context;
        public TrackingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateTracking(Tracking tracking)
        {
            _context.Tracking.Add(tracking);
            return _context.SaveChanges() == 1 ? true : false;
        }

        public ICollection<Tracking> getAllTracking(string dataChangePersonuserId)
        {
           return  _context.Tracking.Where(u=>u.DataChangeUserId == dataChangePersonuserId).ToList();
        }
    }
}
