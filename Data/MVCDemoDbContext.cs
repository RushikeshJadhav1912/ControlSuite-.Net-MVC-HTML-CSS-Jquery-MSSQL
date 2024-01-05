using ASPNETMVCCRUD.Models;
using ASPNETMVCCRUD.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASPNETMVCCRUD.Data
{
    public class MVCDemoDbContext : DbContext
	{
        public MVCDemoDbContext(DbContextOptions<MVCDemoDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }


       /* public DbSet<ContactUs> ContactForms { get; set; }*/





    }
}
