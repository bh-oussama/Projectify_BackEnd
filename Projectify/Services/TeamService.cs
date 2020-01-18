using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projectify.Models;
using System.Diagnostics;
using Projectify.IServices;
using Projectify.Database;

public class TeamService : ITeamService
{

    readonly private ApplicationContext _context;
    readonly private UserManager<ApplicationUser> _userManager;

    public TeamService(ApplicationContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public  IEnumerable<Team> GetTeams(int projectID)
    {
        return _context.Teams.Where(t => t.ProjectID == projectID).ToList();
    }

    public Team CreateTeam(int projectID, string teamName, string teamDescription)
    {
        Project project = _context.Projects.Include(p => p.Teams).Where(p => p.ProjectID == projectID).SingleOrDefault();
        Team newTeam = new Team()
        {
            TeamName = teamName,
            TeamDescription =teamDescription
        };
        newTeam.TeamMembers = new List<ApplicationUser>();
        try
        {
            project.Teams.Add(newTeam);
            _context.Teams.Add(newTeam);
            _context.SaveChanges();
            return newTeam;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.StackTrace);
            return null;
        }
    }

    public bool DeleteTeam(int projectID,int teamID)
    {
        Project project = _context.Projects.Include(p => p.Teams).Where(p => p.ProjectID == projectID).SingleOrDefault();
        Team team = _context.Teams.Where(t => t.TeamID == teamID).SingleOrDefault();
        try
        {
            project.Teams.Remove(team);
            _context.Teams.Remove(team);
            _context.SaveChanges();
        }
        catch(Exception e)
        {
            Debug.WriteLine(e.StackTrace);
            return false;
        }
        return true;

    }
    public bool AddMemberToTeam(int teamID, string memberID)
    {
        Team team = _context.Teams.Include(t => t.TeamMembers).Where(t => t.TeamID == teamID).SingleOrDefault();
        ApplicationUser user = _context.Users.Where(u => u.Id == memberID).SingleOrDefault();
        try
        {
            team.TeamMembers.Add(user);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.StackTrace);
            return false;
        }
        return true;

    }
    public bool RemoveMemberFromTeam(int teamID, string memberID)
    {
        Team team = _context.Teams.Include(t => t.TeamMembers).Where(t => t.TeamID == teamID).SingleOrDefault();
        ApplicationUser user = _context.Users.Where(u => u.Id == memberID).SingleOrDefault();
        try
        {
            team.TeamMembers.Remove(user);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.StackTrace);
            return false;
        }
        return true;

    }
   /* public IEnumerable<ApplicationUser> GetMembersPerTeam(int teamID)
    {
        return _context.Users.Where(u => u.TeamID == teamID).ToList();

    }*/
}
