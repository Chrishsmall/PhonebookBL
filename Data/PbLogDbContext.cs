using Microsoft.EntityFrameworkCore;
using PhonebookBL.Model;

namespace PhonebookBL.Data
{
    public class PbLogDbContext : DbContext
    {
        public PbLogDbContext(DbContextOptions<PbLogDbContext> options) : base(options)
        {

        }
        public DbSet<Log> Logs { get; set; }
    }
}
