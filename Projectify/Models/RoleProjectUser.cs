using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projectify.Models
{
    public class RoleProjectUser
    {
        public static List<string> ROLES = new List<string>() { "Admin", "Manager", "Developper" };
        public int ProjectID { get; set; }
        public string UserID { get; set; }
        public string Role { get; set; }

        public RoleProjectUser() { }
    }
}
