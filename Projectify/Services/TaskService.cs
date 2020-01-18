using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Projectify.IServices;
using Projectify.Models;
using Projectify.Database;

public class TaskService : ITaskService {

    readonly private ApplicationContext _context;
    readonly private UserManager<ApplicationUser> _userManager;

    public TaskService(ApplicationContext context, UserManager<ApplicationUser> userManager) {
        _context = context;
        _userManager = userManager;
    }

    public IEnumerable<Projectify.Models.Task> GetTasks(int projectID)
    {
        Project project = _context.Projects.Include(p => p.Sprints).ThenInclude(s => s.Tasks).Where(p => p.ProjectID == projectID).SingleOrDefault();
        List<int> sprintIDs = _context.Sprints.Where(s => s.ProjectID == project.ProjectID).Select(s => s.SprintID).ToList();
        List<Projectify.Models.Task> tasks = _context.Tasks.Where(t => sprintIDs.Any(s => s == t.SprintID)).ToList();
        return tasks;
    }



    public Projectify.Models.Task CreateTask(int projectID,int sprintID,Projectify.Models.Task task)
    {
        Project project = _context.Projects.Include(s => s.Sprints).ThenInclude(s =>s.Tasks).Where(p => p.ProjectID == projectID).SingleOrDefault();
        Sprint sprint = _context.Sprints.Include(s => s.Tasks).Where(s => s.SprintID == sprintID).SingleOrDefault();
        Debug.WriteLine(sprint);
        Projectify.Models.Task newTask = new Projectify.Models.Task()
        {
            TaskName = task.TaskName,
            TaskPriority = task.TaskPriority,
            TaskDescription = task.TaskDescription,
            TaskStartedAt = task.TaskStartedAt,
            TaskEndedAt = "",
        };
        newTask.Sprint = sprint;

        try
        {
            _context.Tasks.Add(newTask);
            _context.SaveChanges();
            return newTask;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.StackTrace);
            return null;
        }

    }

    public bool ChangeTaskState(int taskID, string taskState)
    {
        Projectify.Models.Task task = _context.Tasks.SingleOrDefault(t => t.TaskID == taskID);

        if (taskState == Projectify.Models.Task.TASK_STATES[2])
        {
            try
            {
                _context.Entry(task).Property(t => t.TaskState).CurrentValue = taskState;
                task.TaskEndedAt = DateTime.Now.ToString("dd/MM/YY");
                _context.SaveChangesAsync();
            }

            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return false;
            }
            return true;
        }
        else
        {
            try
            {
                _context.Entry(task).Property(t => t.TaskState).CurrentValue = taskState;
                _context.SaveChangesAsync();
                return true;
            }

            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return false;
            }
           
        }
    }

    public bool UpdateTask(Projectify.Models.Task task)
    {
        Debug.WriteLine(task.TaskID);
        Debug.WriteLine(task.TaskName);
        Debug.WriteLine(task.TaskPriority);
        Debug.WriteLine(task.TaskState);
        Debug.WriteLine(task.TaskStartedAt);

        Projectify.Models.Task taskToChange = _context.Tasks.Where(t => t.TaskID == task.TaskID).SingleOrDefault();
        taskToChange.TaskDescription = task.TaskDescription;
        taskToChange.TaskName = task.TaskName;
        taskToChange.TaskPriority = task.TaskPriority;
        taskToChange.TaskState = task.TaskState;
        try
        {
            _context.SaveChanges();
            return true;
        }
        catch(Exception e)
        {
            Debug.WriteLine(e.StackTrace);
            return false;
        }

    }

    public bool DeleteTask(int taskID)
    {
      
        Projectify.Models.Task task = _context.Tasks.Where(t => t.TaskID == taskID).SingleOrDefault();
        _context.Tasks.Remove(task);
        try
        {
            _context.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.StackTrace);
            return false;
        }

    }

    public Dictionary<String, String> GetCompletedTasksPerSprint(int projectID)
    {
        Dictionary<string, string> completedTasksPerSprintMap = new Dictionary<string, string>();
        Project project = _context.Projects.Where(p => p.ProjectID == projectID).SingleOrDefault();
        foreach(Sprint sprint in _context.Sprints)
        {
            List<Projectify.Models.Task> tasksPerSprint = _context.Tasks.Where(t => t.SprintID == sprint.SprintID).ToList();
            List<Projectify.Models.Task> completedTasksPerSprint = tasksPerSprint.Where(t => t.TaskState == "Done").ToList();
            completedTasksPerSprintMap.Add(sprint.SprintName, completedTasksPerSprint.Count.ToString());
        }
        return completedTasksPerSprintMap;
    }


    /* public IEnumerable<object> GetAllTasks(int projectID,string userID) {
         ApplicationUser user = _context.Users.Include(u => u.Tasks).SingleOrDefault(u => u.Id == userID);
        // List<ApplicationUser> users = _context.Users.Include(u => u.Team).Where(a => usersID.Any(c => c == a.Id)).Where(u => u.Team == null).ToList();
         Project project = _context.Projects.Include(p => p.Tasks).Include(p => p.Team).ThenInclude(t => t.TeamMembers).Where(p => p.Team.TeamMembers.Any(u => u.Id == userID)).First();
         List<Projectify.Models.Task> tasks = project.Tasks.ToList();
         List<object> activities = new List<object>();
         foreach (Projectify.Models.Task task in tasks) {
             activities.Add(new
             {
                 taskID = task.TaskID,
                 taskName = task.TaskName,
                 taskState = task.TaskState,
                 taskPriority = task.TaskPriority,
                 taskDescription = task.TaskDescription,
                 projectName = _context.Projects.SingleOrDefault(p => p.ProjectID == task.ProjectID).ProjectName

             });
         }
         Debug.Write(activities.Count);
         return activities;
     }
     */

    /* public async Task<IEnumerable<Projectify.Models.Task>> GetTasksOfMemberAsync(string memberID) {
         ApplicationUser user = await _userManager.FindByIdAsync(memberID);
         IEnumerable<Projectify.Models.Task> tasks = user.Tasks.AsEnumerable();
         return tasks;
     }*/

    /*  public int ChangeTaskName(int taskID, string taskName) {
          Projectify.Models.Task task = _context.Tasks.SingleOrDefault(t => t.TaskID == taskID);
          if (task == null) {
              return -1;
          }
          else {
              try {
                  _context.Entry(task).Property(t => t.TaskName).CurrentValue = taskName;
                  _context.SaveChanges();
              }
              catch (Exception e) {
                  Debug.WriteLine(e.StackTrace);
              }
              return 0;
          }
      }



      public int ChangeTaskPriority(int taskID, int taskPriority) {
          Projectify.Models.Task task = _context.Tasks.SingleOrDefault(t => t.TaskID == taskID);
          if (task == null) {
              return -1;
          }
          else {
              if ( (_context.Entry(task).Property(t => t.TaskPriority).CurrentValue != taskPriority) && (taskPriority != 0) ) {
                  try {
                      _context.Entry(task).Property(t => t.TaskPriority).CurrentValue = taskPriority;
                      _context.SaveChanges();
                  }

                  catch (Exception e) {
                      Debug.WriteLine(e.StackTrace);
                  }
              }
              return 0;
          }
      }

      public int ChangeTaskDescription(int taskID, string taskDescription) {
          Projectify.Models.Task task = _context.Tasks.SingleOrDefault(t => t.TaskID == taskID);
          if (task == null) {
              return -1;
          }
          else {
              if ( (_context.Entry(task).Property(t => t.TaskDescription).CurrentValue != taskDescription) && (taskDescription != null) ) {
                  try {
                      _context.Entry(task).Property(t => t.TaskDescription).CurrentValue = taskDescription;
                      _context.SaveChanges();
                  }

                  catch (Exception e) {
                      Debug.WriteLine(e.StackTrace);
                  }
              }
              return 0;
          }
      }*/

    /* public string GetProjectName(int taskID) {
         Projet_Stage.Models.Task task = _context.Tasks.Include(t => t.Project).SingleOrDefault(t => t.TaskID == taskID);
         if (task == null) {
             return "";
         }
         else {
             return task.Project.ProjectName;
         }
     }*/

}

