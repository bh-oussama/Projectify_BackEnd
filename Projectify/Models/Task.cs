using Projectify.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Projectify.Models
{
    public class Task
    {

        public static List<string> TASK_STATES = new List<string>() { "Pending", "InProgress", "Done" };
        [ForeignKey("Sprint")]
        public virtual int SprintID { get; set; }
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public string TaskPriority { get; set; }
        public string TaskDescription { get; set; }
        public string TaskState { get; set; }
        public string TaskStartedAt { get; set; }
        public string TaskEndedAt { get; set; }

        [JsonIgnore]
        public virtual Sprint Sprint { get; set; }

        public Task()
        {
            TaskState = Task.TASK_STATES[0];
            Sprint = new Sprint();

        }

    }

}

