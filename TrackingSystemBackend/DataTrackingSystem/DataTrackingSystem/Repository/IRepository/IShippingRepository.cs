using DataTrackingSystem.Data.Models;

namespace DataTrackingSystem.Repository.IRepository
{
    public interface IShippingRepository
    {
        public ICollection<Shipping> GetShippings( string senderId);
        public Shipping GetShipping(int shippingId);
        public bool CreateShipping(Shipping shipping);
        public bool UpdateShipping(Shipping shipping);
        public bool DeleteShipping(int shippingId);
        public bool SaveShipping();

    }
}
