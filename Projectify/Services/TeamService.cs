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

    public Team CreateTeam(Project project, string teamName, string teamDescription)
    {/*
        Project project = _context.Projects.SingleOrDefault(p => p.ProjectID == projectID);
        if (project == null)
        {
            Team team = project.Teams.SingleOrDefault(t => t.TeamName == teamName);
            if (team == null)
            {*/
                Team newTeam = new Team(teamName, teamDescription);
                newTeam.TeamMembers = new List<ApplicationUser>();
                newTeam.Project = project;
                try
                {
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
}
