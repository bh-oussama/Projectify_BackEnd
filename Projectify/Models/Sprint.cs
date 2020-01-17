using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projectify.Models
{
    public class Sprint
    {
        public int SprintID { get; set; }
        public string SprintName { get; set; }
        public string SprintDateStart { get; set; }
        public string SprintDateEnd { get; set; }
        public virtual List<Task> Tasks { get; set; }
        public virtual Project Project{ get; set; }

        public Sprint() {
            Tasks = new List<Task>();
            Project = new Project();
        }
}
}
