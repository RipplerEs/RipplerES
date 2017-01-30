using Microsoft.EntityFrameworkCore;

namespace RipplerES.EFCoreRepository
{
    public class EventContext : DbContext
    {
        public EventContext(DbContextOptions<EventContext> contextOptions)
            : base(contextOptions)
        {

        }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasIndex(c => new { c.Id, c.Version })
                .HasName("Ix_Event_Identity")
                .IsUnique();
        }
    }
}