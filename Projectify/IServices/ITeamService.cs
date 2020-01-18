using Projectify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projectify.IServices
{
    public interface ITeamService {
        public IEnumerable<Team> GetTeams(int projectID);
        public Team CreateTeam(int projectID, string teamName, string teamDescription);
        public bool DeleteTeam(int projectID,int teamID);
        public bool AddMemberToTeam(int teamID, string memberID);
        public bool RemoveMemberFromTeam(int teamID, string memberID);
      //  public IEnumerable<ApplicationUser> GetMembersPerTeam(int teamID);

    }
}
