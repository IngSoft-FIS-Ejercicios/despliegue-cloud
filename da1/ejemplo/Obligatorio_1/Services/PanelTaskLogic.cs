using IRepository;
using Domain;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Logic;

public class PanelTaskLogic
{
    private IRepositoryFull<PanelTask> _repoTask;
    private IRepository<Comment>  _repoComment;

    public PanelTaskLogic(IRepository<Comment> repoComment, IRepositoryFull<PanelTask> repoTask)
    {
        _repoTask = repoTask;
        _repoComment = repoComment;
    }
    public void AddComment(PanelTask task, Comment comment)
    {
        task.addComment(comment);
        _repoComment.Add(comment);
    }
    
    public void ResolveComment(Comment comment,User user)
    {
        if (comment.Resolved)
        {
            throw new ArgumentException("Comment has already been resolved");
        }
        Comment commentResolved = new Comment()
        {
            Id = comment.Id,
            Description = comment.Description,
            CreatorId = comment.CreatorId,
            Resolved = true,
            ResolverId = user.Id,
            DateResolution = DateTime.Now
        };
        _repoComment.Modify(comment,commentResolved);
    }
    
    public Comment FindCommentById(int id)
    {
        Comment comment = _repoComment.FindById(id);
        
        if (comment == null)
        {
            throw new ArgumentException("Comment not found");
        }

        return comment;
    }

    
    public void InvestTime(PanelTask task, int time)
    {
        task.InvestTime(time);
        _repoTask.Modify(task, task);
    }
    
    public void MarkAsDone(PanelTask task)
    {
        task.FinishTask();
        _repoTask.Modify(task, task);
    }


}