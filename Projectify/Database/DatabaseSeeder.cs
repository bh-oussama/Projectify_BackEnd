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

        ApplicationUser user1 = new ApplicationUser()
        {
            UserName = "user1",
            SecurityStamp = Guid.NewGuid().ToString(),
            Email = "user1@user1.com"
        };
        if (!Usermanager.Users.Any(x => x.Email == user1.Email))
        {
            Usermanager.CreateAsync(user1, "Password@123").Wait();
        }

        ApplicationUser user2 = new ApplicationUser()
        {
            UserName = "user2",
            SecurityStamp = Guid.NewGuid().ToString(),
            Email = "user2@user2.com"
        };
        if (!Usermanager.Users.Any(x => x.Email == user2.Email))
        {
            Usermanager.CreateAsync(user2, "Password@1234").Wait();
        }

        ApplicationUser user3 = new ApplicationUser()
        {
            UserName = "user3",
            SecurityStamp = Guid.NewGuid().ToString(),
            Email = "user3@user3.com"
        };
        if (!Usermanager.Users.Any(x => x.Email == user3.Email))
        {
            Usermanager.CreateAsync(user3, "Password@12345").Wait();
        }

        ApplicationUser user4 = new ApplicationUser()
        {
            UserName = "user4",
            SecurityStamp = Guid.NewGuid().ToString(),
            Email = "user4@user4.com"
        };
        if (!Usermanager.Users.Any(x => x.Email == user4.Email))
        {
            Usermanager.CreateAsync(user4, "Password@123456").Wait();
        }


    }

    public static async System.Threading.Tasks.Task AssigneRoles(IServiceProvider serviceProvider)
    {
        var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    }
}
