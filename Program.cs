using ASPNETMVCCRUD.Areas.Identity.Data;
using ASPNETMVCCRUD.Data;
using ASPNETMVCCRUD.Migrations.AuthDb;
using ASPNETMVCCRUD.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


public class Program
{
    public static async Task Main(String[] args)
    {

        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddDbContext<MVCDemoDbContext>(options => options.UseSqlServer
        (builder.Configuration.GetConnectionString("MvcDemoConnectionString")));

        builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer
        (builder.Configuration.GetConnectionString("MvcDemoConnectionString")));


        builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();



        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddRazorPages();


        var app = builder.Build();




        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapRazorPages();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "SuperAdmin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role)) await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


            try {
                string FirstName = "Admin";
                string LastName = "Admin";
                
                string email = "Admin@gmail.com";
                string password = "Admin@123";
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new ApplicationUser();
                    user.UserName = email;
                    user.Email = email;
                    user.FirstName = FirstName;
                    user.LastName = LastName;


                    await userManager.CreateAsync(user, password);


                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                // Log or examine innerException for details
                throw; // Rethrow the exception if needed
            }

            try
            {
                string FirstName = "SuperAdmin";
                string LastName = "SuperAdmin";

                string email = "SuperAdmin@gmail.com";
                string password = "Super@456";
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new ApplicationUser();
                    user.UserName = email;
                    user.Email = email;
                    user.FirstName = FirstName;
                    user.LastName = LastName;


                    await userManager.CreateAsync(user, password);


                    await userManager.AddToRoleAsync(user, "SuperAdmin");
                }
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                // Log or examine innerException for details
                throw; // Rethrow the exception if needed
            }






            app.Run();
        }
    }
}