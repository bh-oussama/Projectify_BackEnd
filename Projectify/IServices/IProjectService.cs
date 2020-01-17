using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projectify.Models;

namespace Projectify.IServices
{
    public interface IProjectService
    {
         IEnumerable<Project> GetProjectsPerUser(string userID);
         Project CreateProject(string userID,string projectName,string projectDescription);
         int DeleteProject(int projectID);
         Project getProject(int projectID);
        // int ChangeProjectName(int projectId, string projectName);
        // int ChangeProjectState(int projectId, string projectState);
        // int AddMembersToProject(int projectID, List<ApplicationUser> members);
        // int RemoveMemberFromProject(int projectID, string memberID);
        // Projectify.Models.Task AddTaskToProject(int projectId, Projectify.Models.Task task);
        // int RemoveTaskFromProject(int projectID, int TaskID);
    }
}

