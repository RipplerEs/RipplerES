using Microsoft.EntityFrameworkCore;

namespace RipplerAccountTest
{
    public class ViewDataContex : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectsV13;Initial Catalog=RipplesES-EventStore;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=True");
        }

        public DbSet<AccountSummaryView.AccountSummaryView> AccountSummaryViews { get; set; }
    }
}