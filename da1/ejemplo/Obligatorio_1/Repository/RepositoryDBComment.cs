namespace Repository;
using IRepository;
using Domain;

public class RepositoryDBComment : IRepository<Comment>
{

    private readonly SqlContext _database;
    
    public RepositoryDBComment(SqlContext database)
    {
        _database = database;
    }
    public void Add(Comment entity)
    {
        if(entity == null) throw new ArgumentException("Comment cannot be null");
        _database.Comments.Add(entity);
        _database.SaveChanges();
    }

    public void Modify(Comment entity, Comment modifiedEntity)
    {
        var existingEntity = FindById(entity.Id);
        existingEntity.Resolved = modifiedEntity.Resolved;
        existingEntity.Description = modifiedEntity.Description;
        existingEntity.CreatorId = modifiedEntity.CreatorId;
        existingEntity.DateResolution = modifiedEntity.DateResolution;
        existingEntity.ResolverId = modifiedEntity.ResolverId;
        _database.Update(existingEntity);
        _database.SaveChanges();
    }

    public List<Comment> GetAll()
    {
        return _database.Comments.ToList();
    }

    public Comment FindById(int? id)
    {
        Comment comment = _database.Comments.FirstOrDefault(c => c.Id == id);
        
        return comment;
    }
    
}