namespace Domain;

public class Trashpaper
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<PanelTask> ListTasks { get; set; } = new List<PanelTask>();
    
    public void AddTask(PanelTask task)
    {
        ListTasks.Add(task);
    }
    
    public void RemoveTask(PanelTask task)
    {
        ListTasks.Remove(task);
    }
}