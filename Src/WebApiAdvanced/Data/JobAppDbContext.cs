using Microsoft.EntityFrameworkCore;
using WebApiAdvanced.Model;

namespace WebApiAdvanced.Data
{
    public class JobAppDbContext : DbContext
    {
        public JobAppDbContext(DbContextOptions<JobAppDbContext> options) : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; }
    }
}