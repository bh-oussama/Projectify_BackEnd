using Projectify.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projectify.Models
{
    public class Team
    {
        [ForeignKey("Project")]
        public virtual int ProjectID { get; set; }
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public string TeamDescription { get; set; }
        public virtual Project Project { get; set; }
        public virtual List<ApplicationUser> TeamMembers { get; set; }
        public Team() {
        }
    }
}

