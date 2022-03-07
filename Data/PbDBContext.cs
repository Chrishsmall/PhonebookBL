using Microsoft.EntityFrameworkCore;
using PhonebookBL.Model;

namespace PhonebookBL.Data
{
    public class PbDBContext : DbContext
    {
        public PbDBContext(DbContextOptions<PbDBContext> options) : base(options)
        {
            
        }

        public DbSet<Phonebook> Phonebook { get; set; }
        public DbSet<PbEntry> PbEntry { get; set; }
    }
}
