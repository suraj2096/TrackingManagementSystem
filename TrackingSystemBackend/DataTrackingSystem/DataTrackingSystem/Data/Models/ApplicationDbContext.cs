using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace DataTrackingSystem.Data.Models
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Shipping> Shipping { get; set; }
        public DbSet<Tracking> Tracking { get; set; }   
        public DbSet<Invitation> Invitations { get; set; }
      
    }
}
