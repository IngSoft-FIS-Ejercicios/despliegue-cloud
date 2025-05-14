using IRepository;
using Domain;

namespace Logic;

public class TeamLogic
{
    private IRepositoryFull<Team> _repoTeam;
    
    public TeamLogic(IRepositoryFull<Team> repoTeam)
    {
        _repoTeam = repoTeam;
    }
    
    public Team CreateTeam(User admin, Team team)
    {
        if (admin.Type.ToString() != TypeUser.Admin.ToString())
        {
            throw new ArgumentException("User cannot create teams");
        }
        
        _repoTeam.Add(team);
        return team;
    }

    public void RemoveTeam(User admin, Team team, PanelLogic panelLogic)
    {
        if(admin.Type.ToString() != TypeUser.Admin.ToString())
        {
            throw new ArgumentException("User cannot remove teams");
        }

        int panelCounter = 0;
        foreach (var panel in panelLogic.GetPanels())
        {
            if (panel.Team == team)
            {
                panelCounter++;
            }
        }

        if (panelCounter > 0)
        {
            throw new ArgumentException("You cannot remove a team with active panels");
        }
        
        _repoTeam.Remove(team);
    }

    public void AddMember(int teamId, User adder, User toAdd)
    {
        Team findedTeam = _repoTeam.FindById(teamId);
        
        if(findedTeam.TeamUsersList.Contains(toAdd))
        {
            throw new ArgumentException("User already in team");
        }
        
        if(!findedTeam.Administrator.Equals(adder) && adder.Type.ToString() != TypeUser.Admin.ToString())
        {
            throw new ArgumentException("User cannot add members to team");
        }

        findedTeam.TeamUsersList.Add(toAdd);
        _repoTeam.Modify(findedTeam,findedTeam);
        
    }
    
    public void RemoveMember(Team team, User remover, User toRemove)
    {
        Team? findedTeam = _repoTeam.FindById(team.Id);
        
        if(!findedTeam.TeamUsersList.Contains(toRemove))
        {
            throw new ArgumentException("User not in team");
        }
        
        if(!findedTeam.Administrator.Equals(remover) && remover.Type.ToString() != TypeUser.Admin.ToString())
        {
            throw new ArgumentException("User cannot remove members from team");
        }

        if (toRemove.Equals(team.Administrator))
        {
            throw new ArgumentException("Administrator cannot be removed from team");
        }
        
        findedTeam.TeamUsersList.Remove(toRemove);
        _repoTeam.Modify(findedTeam,findedTeam);
    }

    public void ModifyTeam(User modifier, Team team1, Team team2)
    {
        if(!team1.Administrator.Equals(modifier) && modifier.Type.ToString() != TypeUser.Admin.ToString())
        {
            throw new ArgumentException("User cannot modify team");
        }
        
        _repoTeam.Modify(team1, team2);
    }

    public bool CheckUserIsAdmin(User user)
    {
        foreach (var team in _repoTeam.GetAll())
        {
            if(team.Administrator.Equals(user))
            {
                return true;
            }
        }
        return false;
    }
    
    public Team FindTeamById(int? id)
    {
        Team? team = _repoTeam.FindById(id);
        
        return team;
    }
    
    public List<Team> GetTeams()
    {
        return _repoTeam.GetAll();
    }
}