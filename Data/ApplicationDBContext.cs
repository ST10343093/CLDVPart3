using Microsoft.EntityFrameworkCore;

namespace CLDVPart3.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        
    }
}
