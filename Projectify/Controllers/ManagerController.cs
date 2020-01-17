using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projectify.Models;
using Projectify.Database;
using Projectify.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Projectify.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _singInManager;
        private readonly IConfiguration _configuration;
        private RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;
        private readonly IProjectService _projectService;
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;
        private readonly ITeamService _teamService;


        public ManagerController(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _singInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
            _configuration = configuration;
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _context = serviceProvider.GetRequiredService<ApplicationContext>();
            _projectService = new ProjectService(_context, _userManager);
            _taskService = new TaskService(_context, _userManager);
            _userService = new UserService(_context, _userManager);
            _teamService = new TeamService(_context, _userManager);
        }


    }
}