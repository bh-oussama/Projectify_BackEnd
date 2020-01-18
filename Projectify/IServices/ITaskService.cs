using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Projectify.IServices
{
    public interface ITaskService {
        Projectify.Models.Task CreateTask(int projectID, int sprintID,Projectify.Models.Task task);
        bool ChangeTaskState(int taskID, string taskState);
        bool UpdateTask(Projectify.Models.Task task);
        IEnumerable<Projectify.Models.Task> GetTasks(int projectID);
        bool DeleteTask(int taskID);
        Dictionary<String, String> GetCompletedTasksPerSprint(int projectID);
 
    }
}

