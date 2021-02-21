using arm_hello_world.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace arm_hello_world.Db
{
    public class ArmContext : DbContext
    {
        public DbSet<ArmItem> ArmItem { get; set; }
        
        public ArmContext(DbContextOptions<ArmContext> options) : base(options) {}
    }
}