using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projectify.Models
{
    public class Project
    {
        public static List<string> PROJECT_STATE = new List<string>(){ "Completed", "InProgress" };
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectState { get; set; }
        public string ProjectDescription { get; set; }

        public virtual List<Sprint> Sprints { get; set; }
        public virtual List<Team> Teams { get; set; }

      /*  public Project(string projectName, string projectState,string projectDescription) {
            ProjectName = projectName;
            ProjectState = projectState;
            ProjectDescription = projectDescription;
            Teams = new List<Team>();
            Sprints = new List<Sprint>();
        }*/

        public Project() {
            Sprints = new List<Sprint>();
            Teams = new List<Team>();
        }
       
    }
}
