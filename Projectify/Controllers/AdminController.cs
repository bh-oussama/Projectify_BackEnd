using Microsoft.AspNetCore.Authorization;
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

    public AdminController(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        _singInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
        _configuration = configuration;
        _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        _context = serviceProvider.GetRequiredService<ApplicationContext>();
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
            result = project
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
    









        /* [Authorize]
         [HttpPut]
         [Route("changeprojectname")]
         public IActionResult ChangeProjectName([FromQuery(Name = "projectID")] int projectID, [FromBody] string projectName)
         {
             switch (_projectService.ChangeProjectName(projectID, projectName))
             {
                 case 0:
                     return Ok(new
                     {
                         state = "Project's name changed successfully"
                     });
                 default:
                     return BadRequest(new
                     {
                         state = "Something went wrong"
                     });
             }
         }

         [Authorize]
         [HttpPut]
         [Route("changeprojectstate")]
         public IActionResult ChangeProjectState([FromQuery(Name = "projectID")] int projectID, [FromBody] string projectState)
         {
             switch (_projectService.ChangeProjectState(projectID, projectState))
             {
                 case 0:
                     return Ok(new
                     {
                         state = "Project's state changed successfully"
                     });
                 default:
                     return BadRequest(new
                     {
                         state = "Something went wrong"
                     });
             }
         }*/
    }

