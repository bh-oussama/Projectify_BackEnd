using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Projectify.Database;
using Projectify.IServices;
using Projectify.Models;
using Projectify.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

[Route("[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _singInManager;
    private readonly IConfiguration _configuration;
    private RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationContext _context;
    private readonly IProjectService _projectService;
    private readonly ITaskService _taskService;
    private readonly ITeamService _teamService;
    private readonly ISprintService _sprintService;
    private IHttpContextAccessor _httpContextAccessor;

    public AdminController(IServiceProvider serviceProvider, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        _singInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
        _configuration = configuration;
        _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        _context = serviceProvider.GetRequiredService<ApplicationContext>();
        _httpContextAccessor = httpContextAccessor;
        _projectService = new ProjectService(_context, _userManager);
        _taskService = new TaskService(_context, _userManager);
        _teamService = new TeamService(_context, _userManager);
        _sprintService = new SprintService(_context, _userManager);

    }

    [Authorize]
    [HttpPost]
    [Route("createproject")]
    public IActionResult CreateProject([FromQuery(Name = "userID")] string userID, [FromBody] Object obj)
    {

        Project project = JsonSerializer.Deserialize<Project>(obj.ToString());
        Project project1 = _context.Projects.SingleOrDefault(p => p.ProjectName == project.ProjectName);
        if (project1 == null)
        {
            Project newProject = _projectService.CreateProject(userID, project.ProjectName, project.ProjectDescription);
            switch (newProject)
            {
                case null:
                    return BadRequest(new
                    {
                        userMessage = "Operation was unsuccessful",
                        errorCode = "Something went wrong"
                    });
                default:
                    return Created("", new
                    {
                        projectID = newProject.ProjectID

                    });
            };
        }
        else
        {
            return BadRequest(new
            {
                state = "Something went wrong"
            });

        }
    }


    [Authorize]
    [HttpGet]
    [Route("getprojectsperuser")]
    public IActionResult GetProjectsPerUser([FromQuery(Name = "userID")] string userID)
    {
        IEnumerable<Project> projects = _projectService.GetProjectsPerUser(userID);
        return Ok(new
        {
            count = projects.Count(),
            result = projects
        });
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteproject")]
    public IActionResult DeleteProject([FromQuery(Name = "projectID")] int projectId)
    {

        switch (_projectService.DeleteProject(projectId))
        {
            case 0:
                return Ok(new
                {
                    state = "Project deleted successfully"
                });
            default:
                return BadRequest(new
                {
                    userMessage = "Operation was unsuccessful",
                    errorCode = "Something went wrong"
                });
        }
    }

    [Authorize]
    [HttpDelete]
    [Route("deletetask")]
    public IActionResult DeleteTask([FromQuery(Name = "taskID")] int taskID)
    {

        switch (_taskService.DeleteTask(taskID))
        {
            case true:
                return Ok(new
                {
                    state = "Task deleted successfully"
                });
            default:
                return BadRequest(new
                {
                    userMessage = "Operation was unsuccessful",
                    errorCode = "Something went wrong"
                });
        }
    }



    [Authorize]
    [HttpPost]
    [Route("createtask")]
    public IActionResult CreateTask([FromQuery(Name = "projectID")]int projectID, [FromQuery(Name = "sprintID")]int sprintID, [FromBody] Object obj)
    {
        Task task = JsonSerializer.Deserialize<Task>(obj.ToString());
        Task newTask = _taskService.CreateTask(projectID, sprintID, task);
        switch (newTask)
        {
            case null:
                return BadRequest(new
                {
                    userMessage = "Operation was unsuccessful",
                    errorCode = "Something went wrong"
                });
            default:
                return Created("", new
                {
                    taskID = newTask.TaskID

                });
        };
    }

    [Authorize]
    [HttpPost]
    [Route("createsprint")]
    public IActionResult CreateSprint([FromQuery(Name = "projectID")]int projectID,[FromBody] Object obj)
    {
        Sprint sprint = JsonSerializer.Deserialize<Sprint>(obj.ToString());
        Sprint newSprint = _sprintService.CreateSprint(projectID, sprint.SprintName, sprint.SprintDateStart, sprint.SprintDateEnd);
        switch (newSprint)
        {
            case null:
                return BadRequest(new
                {
                    userMessage = "Operation was unsuccessful",
                    errorCode = "Something went wrong"
                });
            default:
                return Created("", new
                {
                    sprintID =newSprint.SprintID

                });
        };
    }

    [Authorize]
    [HttpPost]
    [Route("updatetask")]
    public IActionResult UpdateTask([FromBody] Object obj)
    {
        Projectify.Models.Task task = JsonSerializer.Deserialize<Task>(obj.ToString());
        bool response = _taskService.UpdateTask(task);
        switch (response)
        {
            case false:
                return BadRequest(new
                {
                    userMessage = "Operation was unsuccessful",
                    errorCode = "Something went wrong"
                });
            default:
                return NoContent();
        };
    }

    [Authorize]
    [HttpGet]
    [Route("getproject")]
    public IActionResult GetProjects([FromQuery(Name = "projectID")]int projectID)
    {
        Project project = _projectService.getProject(projectID);
        return Ok(new
        {  
            result = project,
            sprints = project.Sprints
        });
    }

    [Authorize]
    [HttpGet]
    [Route("getusers")]
    public IActionResult GetUsers()
    {
        string userID = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
        IEnumerable<ApplicationUser> users = _context.Users.Where(u => u.Id != userID);
        return Ok(new
        {
            result = users
        });
    }
   
    [Authorize]
    [HttpGet]
    [Route("gettasks")]
    public IActionResult GetTasks([FromQuery(Name = "projectID")]int projectID)
    {
        IEnumerable<Task> tasks = _taskService.GetTasks(projectID);
        return Ok(new
        {
            result = tasks
        });
    }

    [Authorize]
    [HttpPost]
    [Route("changetaskstate")]
    public IActionResult ChangeTaskState([FromQuery(Name = "taskID")]int taskID,[FromBody] string taskState)
    {
      
        bool response = _taskService.ChangeTaskState(taskID, taskState);
        switch (response)
        {
            case false:
                return BadRequest(new
                {
                    userMessage = "Operation was unsuccessful",
                    errorCode = "Something went wrong"
                });
            default:
                return Ok();
        };
    }

    [Authorize]
    [HttpGet]
    [Route("getteams")]
    public IActionResult GetTeams([FromQuery(Name = "projectID")]int projectID)
    {
        IEnumerable<Team> teams = _teamService.GetTeams(projectID);
        return Ok(new
        {
            result = teams
        });
    }

    [Authorize]
    [HttpGet]
    [Route("getmembersperteam")]
    public IActionResult GetMembersPerTeam([FromQuery(Name = "teamID")]int teamID)
    {
        IEnumerable<Team> teams = _teamService.GetTeams(teamID);
        return Ok(new
        {
            result = teams
        });
    }

    [Authorize]
    [HttpGet]
    [Route("getsprints")]
    public IActionResult GetSprints([FromQuery(Name = "projectID")]int projectID)
    {
        IEnumerable<Sprint> sprints = _sprintService.GetSprints(projectID);
        return Ok(new
        {
            result = sprints.ToList()
        });
    }

    [Authorize]
    [HttpPost]
    [Route("createteam")]
    public IActionResult CreateTeam([FromQuery(Name = "projectID")]int projectID, Object obj)
    {
        Team team = JsonSerializer.Deserialize<Team>(obj.ToString());
         Team newTeam = _teamService.CreateTeam(projectID, team.TeamName, team.TeamDescription);
         switch (newTeam)
         {
             case null:
                 return BadRequest(new
                 {
                     userMessage = "Operation was unsuccessful",
                     errorCode = "Something went wrong"
                 });
             default:
                 return Created("", new
                 {
                     teamID = newTeam.TeamID

                 });
         };
    }
    [Authorize]
    [HttpPost]
    [Route("removememberfromteam")]
    public IActionResult RemoveMemberFromTeam([FromQuery(Name = "teamID")]int teamID, [FromQuery(Name = "memberID")]string memberID)
    {
        bool response = _teamService.RemoveMemberFromTeam(teamID, memberID);

        switch (response)
        {
            case false:
                return BadRequest(new
                {
                    userMessage = "Operation was unsuccessful",
                    errorCode = "Something went wrong"
                });
            default:
                return Ok();
        };
    }
     
    [Authorize]
    [HttpPost]
    [Route("addmembertoteam")]
    public IActionResult AddMemberToTeam([FromQuery(Name = "teamID")]int teamID, [FromQuery(Name = "memberID")]string memberID)
    {
        bool response = _teamService.AddMemberToTeam(teamID, memberID);


        switch (response)
        {
            case false:
                return BadRequest(new
                {
                    userMessage = "Operation was unsuccessful",
                    errorCode = "Something went wrong"
                });
            default:
                return Ok();
        };
    }

    [Authorize]
    [HttpGet]
    [Route("getctaskspersprint")]
    public IActionResult GetCompletedTasksPerSprint([FromQuery(Name = "projectID")]int projectID)
    {
        Dictionary<string,string> response = _taskService.GetCompletedTasksPerSprint(projectID);
      

                return Ok(new
                {
                    result = response
                });
   
    }


}

