using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Projectify.Models
{
    public class Sprint
    {
        [ForeignKey("Project")]
        public virtual int ProjectID { get; set; }
        public int SprintID { get; set; }
        public string SprintName { get; set; }
        public string SprintDateStart { get; set; }
        public string SprintDateEnd { get; set; }
        [JsonIgnore]
        public virtual List<Task> Tasks { get; set; }
      
        [JsonIgnore]
        public virtual Project Project{ get; set; }

        public Sprint() {
            Tasks = new List<Task>();
            Project = new Project();
        }
}
}
