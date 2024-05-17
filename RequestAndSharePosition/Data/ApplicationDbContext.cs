using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RequestAndSharePosition.Shared;

namespace RequestAndSharePosition.Data
{
    public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<IdentityUser>(options)
    {
        public virtual DbSet<Request> Requests { get; init; }
        public virtual DbSet<SharingGroup> SharingGroups { get; init; }
    }
}
