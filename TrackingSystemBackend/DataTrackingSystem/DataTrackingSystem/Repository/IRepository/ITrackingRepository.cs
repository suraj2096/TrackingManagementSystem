using DataTrackingSystem.Data.Models;

namespace DataTrackingSystem.Repository.IRepository
{
    public interface ITrackingRepository
    {
        public ICollection<Tracking> getAllTracking(string dataChangePersonuserId);
        public bool CreateTracking(Tracking tracking);
    }
}
