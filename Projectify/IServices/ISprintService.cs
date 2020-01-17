using Projectify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projectify.IServices
{
    interface ISprintService
    {
        public Sprint CreateSprint(int projectID, string sprintName, string sprintDateStart, string sprintDateEnd);
    }
}
