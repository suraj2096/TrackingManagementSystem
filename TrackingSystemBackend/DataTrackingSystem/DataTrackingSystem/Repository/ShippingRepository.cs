using DataTrackingSystem.Data.Models;
using DataTrackingSystem.Repository.IRepository;
using System.Diagnostics.CodeAnalysis;

namespace DataTrackingSystem.Repository
{
    public class ShippingRepository : IShippingRepository
    {
        private readonly ApplicationDbContext _context;
        public ShippingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CreateShipping(Shipping shipping)
        {
            _context.Add(shipping);
            return SaveShipping();
        }

        public bool DeleteShipping(int shippingId)
        {
            var findShipping = GetShipping(shippingId);
            if (findShipping == null) return false;
            var delShippping = _context.Shipping.Remove(findShipping);
            if(delShippping == null) return false;
            SaveShipping();
            return true;
        }

        public Shipping GetShipping(int shippingId)
        {
           var data = _context.Shipping.FirstOrDefault(u=>u.ShippingId== shippingId);
            return data;
        }

        public ICollection<Shipping> GetShippings( string senderId)
        {
            return _context.Shipping.Where(u=>u.UserId == senderId).ToList();
        }

        public bool UpdateShipping(Shipping shipping)
        {
           var updateShipping = _context.Shipping.Update(shipping);
            if (updateShipping == null) return false;
            SaveShipping();
            return true;
        }
        public bool SaveShipping()
        {
            return _context.SaveChanges() == 1 ? true : false;
        }

    }
}
