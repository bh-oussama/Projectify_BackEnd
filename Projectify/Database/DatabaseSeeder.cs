using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projectify.Models;
using Projectify.Database;

public class DatabaseSeeder
{
    public static async System.Threading.Tasks.Task CreateRoles(IServiceProvider serviceProvider, IConfiguration Configuration)
    {
        var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        string[] roleNames = { "Admin", "Manager", "Developer" };
        IdentityResult result;
        foreach (var roleName in roleNames)
        {
            // creating the roles and seeding them to the database
            var roleExist = await RoleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                result = await RoleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    public static void Initialize(IServiceProvider serviceProvider)
    {
        var Context = serviceProvider.GetRequiredService<ApplicationContext>();
        var Usermanager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var Rolemanager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        ApplicationUser admin = new ApplicationUser()
        {
            UserName = "admin",
            SecurityStamp = Guid.NewGuid().ToString(),
            Email = "admin@admin.com"
        };
        if (!Usermanager.Users.Any(x => x.Email == admin.Email))
        {
            Usermanager.CreateAsync(admin, "Password@1").Wait();
        }

        
    }

    public static async System.Threading.Tasks.Task AssigneRoles(IServiceProvider serviceProvider)
    {
        var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    }
}
