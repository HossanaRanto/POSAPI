using Microsoft.EntityFrameworkCore;
using POSAPI.Model;

namespace POSAPI.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<UserClient> UserClients { get; set; }
        public DbSet<UserEmployee> UserEmployees { get; set; }
        public DbSet<UserProfil> UserProfils { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Produit> Products { get; set; }
    }
}
