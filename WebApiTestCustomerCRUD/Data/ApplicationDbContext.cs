using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiTestCustomerCRUD.Models;

namespace WebApiTestCustomerCRUD.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
