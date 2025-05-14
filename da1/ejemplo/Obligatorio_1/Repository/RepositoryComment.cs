using System.Dynamic;
using Domain;
using IRepository;

namespace Repository;

public class RepositoryComment : IRepository<Comment>{

    private int _idGenerated = 1;
    public List<Comment> Comments { get; set; } = new List<Comment>();
    
    public void Add(Comment entity)
    {
        if(entity == null)
        {
            throw new ArgumentException("Comment cannot be null");
        }
        entity.Id = GetId();
        Comments.Add(entity);
    }
    
    public void Modify(Comment entity, Comment modifiedEntity)
    {
        entity.Description = modifiedEntity.Description;
        entity.Resolved = modifiedEntity.Resolved;
        entity.DateResolution = modifiedEntity.DateResolution;
        entity.ResolverId = modifiedEntity.ResolverId;
    }

    public List<Comment> GetAll()
    {
        return Comments;
    }

    public Comment FindById(int? id)
    {
        Comment comment = Comments.FirstOrDefault(c => c.Id == id);
        
        return comment;
    }

    public int GetId()
    {
        return _idGenerated++;
    }
}