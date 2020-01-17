using Projectify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projectify.IServices
{
    public interface ITeamService {
        public Team CreateTeam(Project project, string teamName, string teamDescription);
    }
}
