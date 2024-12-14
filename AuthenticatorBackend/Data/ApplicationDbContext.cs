using AuthenticatorBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticatorBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}