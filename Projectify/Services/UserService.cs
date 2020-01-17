using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projectify.Models;
using System.Diagnostics;
using Projectify.IServices;
using Projectify.Database;

public class UserService : IUserService
{

    readonly private ApplicationContext _context;
    readonly private UserManager<ApplicationUser> _userManager;

    public UserService(ApplicationContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


   /* public IEnumerable<ApplicationUser> GetAvailableDevelopers()
    {
        string roleID = _context.Roles.Where(r => r.Name == "Developer").Select(r => r.Id).Distinct().FirstOrDefault();
        List<string> usersID = _context.UserRoles.Where(r => r.RoleId == roleID).Select(u => u.UserId).Distinct().ToList();
        List<ApplicationUser> users = _context.Users.Include(u => u.Team).Where(a => usersID.Any(c => c == a.Id)).Where(u => u.Team == null).ToList();
        return users;
    }*/

    public IEnumerable<ApplicationUser> GetUsers()
    {
        return _context.Users.ToList();
    }
}