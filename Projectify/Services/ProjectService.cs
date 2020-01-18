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


public class ProjectService : IProjectService
{

    readonly private ApplicationContext _context;
    readonly private UserManager<ApplicationUser> _userManager;

    public ProjectService(ApplicationContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IEnumerable<Project> GetProjectsPerUser(string userID)
    {
        List<int> projectIDs = _context.RoleProjectUsers.Where(p => p.UserID == userID).Select(p => p.ProjectID).ToList();
        List<Project> projects = _context.Projects.Where(p => projectIDs.Any(i => i == p.ProjectID)).ToList();
        return projects;
    }

    public Project CreateProject(string userID,string projectName, string projectDescription)
    {
        Project newProject = new Project()
        {
            ProjectName = projectName,
            ProjectState = Project.PROJECT_STATE[1],
            ProjectDescription = projectDescription
        };
        newProject.Teams = new List<Team>();

        try
        {
          
           
            _context.Projects.Add(newProject);
            _context.SaveChanges();
            RoleProjectUser role = new RoleProjectUser()
            {
                ProjectID = newProject.ProjectID,
                UserID = userID,
                Role = RoleProjectUser.ROLES[0]
            };
            _context.RoleProjectUsers.Add(role);
            _context.SaveChanges();
            return newProject;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.StackTrace);
            return null;
        }
    }

  
    public int DeleteProject(int projectID)
    {
        Project project = _context.Projects.Where(p => p.ProjectID == projectID).SingleOrDefault();
        try
        {
            _context.Projects.Remove(project);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.StackTrace);
            return -1;
        }
        return 0;
    }


    public Project getProject(int projectID)
    {
        
        return _context.Projects.Include(p => p.Sprints).Where(p => p.ProjectID == projectID).SingleOrDefault();
        //Include(p => p.Teams).Include(p => p.Sprints).ThenInclude(sprint => sprint.Tasks)
    }



    /*   public int ChangeProjectState(int projectId, string projectState) {
           Project project = _context.Projects.SingleOrDefault(p => p.ProjectID == projectId);
           if (project == null) {
               return -1;
           }
           else {
               try {
                   _context.Entry(project).Property(u => u.ProjectState).CurrentValue = projectState;
                   _context.SaveChanges();
               }
               catch (Exception e) {
                   Debug.WriteLine(e.StackTrace);
               }
               return 0;
           }
       }

       public int AddMembersToProject(int projectID, List<ApplicationUser> members) {
           Project project = _context.Projects.Include(p => p.Team).SingleOrDefault(p => p.ProjectID == projectID);
           List<ApplicationUser> users = _context.Users.ToList();
           try {
               foreach (ApplicationUser member in members) {
                   users.SingleOrDefault(u => u.Id == member.Id).Team = project.Team;
               }
               _context.SaveChanges();
           }
           catch (Exception e) {
               Debug.WriteLine(e.StackTrace);
               return -1;
           }
           return 0;
       }

       public int RemoveTaskFromProject(int projectID, int taskID) {
           Project project = _context.Projects.Include(p => p.Tasks).SingleOrDefault(p => p.ProjectID == projectID);
           Projet_Stage.Models.Task task = project.Tasks.Find(t => t.TaskID == taskID);                               
           if ( (task == null) || (project == null) ) {
               return -1;
           }
           else {
               project.Tasks.Remove(task);
               _context.SaveChanges();
               return 0;
           }
       }

       public Projet_Stage.Models.Task AddTaskToProject(int projectId, Projet_Stage.Models.Task task) {
           Project project = _context.Projects.SingleOrDefault(p => p.ProjectID == projectId);
           if (project == null) {
               return null;
           }
           Projet_Stage.Models.Task newTask = new Projet_Stage.Models.Task
           {
               TaskName = task.TaskName,
               TaskPriority = task.TaskPriority,
               TaskDescription = task.TaskDescription,
               TaskState = "New"
           };
           newTask.ProjectID = projectId;
           newTask.Project = project;
           try {
               _context.Tasks.Add(newTask);
               project.Tasks.Add(newTask);
               _context.SaveChanges();
           }
           catch (Exception e) {
               Debug.WriteLine(e.StackTrace);
           }
           return  newTask ;
       }

       public int RemoveMemberFromProject(int projectID, string memberID) {
           Project project = _context.Projects.SingleOrDefault(p => p.ProjectID == projectID);
           ApplicationUser user = _context.Users.Include(u => u.Team).SingleOrDefault(u => u.Id == memberID);
           if( (project == null) || (user == null)) {
               return -1;
           }
           else {
               user.Team = null;
               _context.SaveChanges();
               return 0;
           }
       }*/

}
