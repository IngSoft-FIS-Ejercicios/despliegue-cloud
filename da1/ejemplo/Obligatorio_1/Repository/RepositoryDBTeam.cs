using Domain;
using IRepository;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RepositoryDBTeam : IRepositoryFull<Team>
{

    private SqlContext _database;
    
    public RepositoryDBTeam(SqlContext context)
    {
        _database = context;
    }
    public void Add(Team entity)
    {
        if(entity == null) throw new ArgumentException("Team cannot be null");
        _database.Teams.Add(entity);
        _database.SaveChanges();
    }

    public void Modify(Team entity, Team modifiedEntity)
    {
        var toModify = FindById(entity.Id);
        toModify.TeamName = modifiedEntity.TeamName;
        toModify.TaskDescription = modifiedEntity.TaskDescription;
        toModify.MaxUsersAllowed = modifiedEntity.MaxUsersAllowed;
        _database.Update(toModify);
        _database.SaveChanges();
    }

    public Team FindById(int? id)
    {
        Team findedTeam = _database.Teams
            .Include(t => t.TeamUsersList)
            .FirstOrDefault(t => t.Id == id);
        
        if(findedTeam == null) throw new ArgumentException("Team not found");
        
        return findedTeam;
    }

    public List<Team> GetAll()
    {
        return _database.Teams
            .Include(t => t.TeamUsersList) 
            .ToList();;
    }

    public void Remove(Team entity)
    {
        var toRemove = FindById(entity.Id);
        _database.Teams.Remove(toRemove);
        _database.SaveChanges();
    }

    public bool Exists(Team entity)
    {
        return _database.Teams.Contains(entity);
    }
}