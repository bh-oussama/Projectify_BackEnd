using Microsoft.AspNetCore.Identity;
using Projectify.Database;
using Projectify.IServices;
using Projectify.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Projectify.Services
{
    public class SprintService: ISprintService
    {
        readonly private ApplicationContext _context;
        readonly private UserManager<ApplicationUser> _userManager;

        public SprintService(ApplicationContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public Sprint CreateSprint(int projectID,string sprintName,string sprintDateStart, string sprintDateEnd)
        {
            Project project = _context.Projects.Where(p => p.ProjectID == projectID).SingleOrDefault();
            Sprint newSprint = new Sprint()
            {
                SprintName = sprintName,
                SprintDateStart = sprintDateStart,
                SprintDateEnd = sprintDateEnd
            };
            newSprint.Project = project;
            try
            {
                _context.Sprints.Add(newSprint);
                _context.SaveChanges();
                return newSprint;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return null;
            }
        }

        public IEnumerable<Sprint> GetSprints(int projectID)
        {
            return _context.Sprints.Where(s => s.ProjectID == projectID);
        }
    }
}
