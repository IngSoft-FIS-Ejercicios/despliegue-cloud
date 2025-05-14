using Domain;
using IRepository;

namespace Repository;

public class RepositoryDBNotification : IRepositoryPreFull<Notification>
{

    private SqlContext _context;
    
    public RepositoryDBNotification(SqlContext context)
    {
        _context = context;
    }


    public void Add(Notification entity)
    {
        _context.Notifications.Add(entity);
        _context.SaveChanges();
    }

    public void Remove(Notification entity)
    {
        Notification toRemove = _context.Notifications.FirstOrDefault(x => x.Id == entity.Id);
        _context.Notifications.Remove(toRemove);
        _context.SaveChanges();

    }
    
    public List<Notification> GetData(User user)
    {
        List<Notification> answer = new List<Notification>();
        List<Notification> notifications = _context.Notifications.ToList();
        foreach (Notification notification in notifications)
        {
            if (notification.UserId == user.Id)
            {
                answer.Add(notification);
            }
        }
        return answer;
    }
}