using System.Globalization;
using Domain;
using IRepository;

namespace Logic;

public class PanelLogic
{
    private IRepositoryFull<Panel> _repoPanel;
    public IRepositoryFull<PanelTask> _repoTask;

    public PanelLogic(IRepositoryFull<Panel> repoPanel, IRepositoryFull<PanelTask> repoTask)
    {
        _repoPanel = repoPanel;
        _repoTask = repoTask;
    }

    public void CreatePanel(Panel panel)
        {
            if(panel.Team == null)
            {
                throw new ArgumentException("Panel must be associated with a team");
            }
            _repoPanel.Add(panel);
            
        }
    
    public void RemovePanel(User creatorOrAdmin, Panel panel)
    {
        if (creatorOrAdmin.Type.ToString() != TypeUser.Admin.ToString() && !panel.Creator.Equals(creatorOrAdmin))
        {
            throw new ArgumentException("Only the creator or an admin can remove a panel");
        }

        for (int i = panel.PanelTaskList.Count - 1; i >= 0; i--)
        {
            RemoveTaskPermanently(creatorOrAdmin, panel, panel.PanelTaskList[i]);
        }
        
        _repoPanel.Remove(panel);
    }

    public void ModifyPanel(Panel toModify, Panel modified)
    {
        _repoPanel.Modify(toModify, modified);
    }

    public void CreateTask(Panel panel, PanelTask task)
    {
        panel.PanelTaskList.Add(task);
        _repoTask.Add(task);
    }
    public void RemoveTaskPermanently(User user, Panel panel, PanelTask task)
    {
        panel.PanelTaskList.Remove(task);
        task.commentList.Clear();
        _repoTask.Remove(task);
    }

    public void CheckExpiredTasks()
    {
        // We iterate over all the panels looking for expired task. Then we remove them from the panel and add them to the first panel (Expired Panel)
        foreach (var panel in _repoPanel.GetAll())
        {
            var expiredTasks = new List<PanelTask>();
            foreach (var task in panel.PanelTaskList)
            {

                if (task.DueDate < DateTime.Now)
                {
                    expiredTasks.Add(task);
                }
            }

            foreach (var task in expiredTasks)
            {
                var expiredPanel = _repoPanel.GetAll()[0]; // We assume that the first panel is the expired panel
                if(!expiredPanel.PanelTaskList.Contains(task)) expiredPanel.PanelTaskList.Add(task);
            }
        }
    }

    public Panel findPanelById(int? id)
    {
        Panel panel = _repoPanel.FindById(id.Value);
        
        return panel;
    }
    
    public PanelTask findTaskById(int id)
    {
        PanelTask task = _repoTask.FindById(id);
        
        return task;
    }

    public void ModifyTask(PanelTask modified, PanelTask modifier)
    {
        _repoTask.Modify(modified, modifier);
    }
    
    public void ReactivateTask(User user, PanelTask task)
    {
        if(user.Type.ToString() != TypeUser.Admin.ToString())
        {
            throw new ArgumentException("Only an admin can reactivate a task");
        }
        
        task.DueDate = DateTime.Now.AddDays(10);
        Panel outdatedPanel = _repoPanel.GetAll()[0];
        outdatedPanel.PanelTaskList.Remove(task);
        _repoTask.Modify(task, task);
    }

    public bool CheckUserIsCreator(User user)
    {
        foreach (var panel in _repoPanel.GetAll())
        {
            if (panel.Creator.Equals(user))
            {
                return true;
            }
        }
        return false;
    }

    public List<Panel> GetPanels()
    {
        return _repoPanel.GetAll();
    }
    
    public List<PanelTask> GetTasks()
    {
        return _repoTask.GetAll();
    }
    
    public bool ExistsPanel(Panel panel)
    {
        return _repoPanel.Exists(panel);
    }
    
    public bool ExistsTask(PanelTask task)
    {
        return _repoTask.Exists(task);
    }
    
    public int CalculateTotalEstimatedTime(Epic epic)
    {
        if (epic == null)
        {
            throw new ArgumentException("Epic cannot be null");
        }

        int totalEstimatedTime = 0;

        foreach (var task in GetTasks())
        {
                if (task.Epic != null && task.Epic.Equals(epic) && task.UserTrashpaperId == null)
                {
                    totalEstimatedTime += task.EstimatedTime;
                }
        }

        return totalEstimatedTime;
    }

    public int CalculateTotalInvestedTime(Epic epic)
    {
        if (epic == null)
        {
            throw new ArgumentException("Epic cannot be null");
        }

        int totalInvestedTime = 0;

        foreach (var task in GetTasks())
        {
            if (task.Epic != null && task.Epic.Equals(epic) && task.UserTrashpaperId == null)
            {
                totalInvestedTime += task.InvestedTime;
            }
        }

        return totalInvestedTime;
    }

    public int CalculateRemainingTime(Epic epic)
    {

        int remainingTime = CalculateTotalEstimatedTime(epic) - CalculateTotalInvestedTime(epic);

        return remainingTime;
    }
    public int CalculateRemainingTimePositive(Epic epic)
    {
        
        if (epic == null)
        {
            throw new ArgumentException("Epic cannot be null");
        }
        
        int remainingTime = 0;
        foreach (var task in GetTasks())
        {
            if (task.Epic != null && task.Epic.Equals(epic) && task.State == StateTask.Unfinished && task.UserTrashpaperId == null)
            {
                remainingTime += Math.Max(0,task.EstimatedTime - task.InvestedTime);
            }
        }
        return remainingTime;
    }
    public int CalculateTotalOverdueHours(Epic epic)
    {
        if (epic == null)
        {
            throw new ArgumentException("Epic cannot be null");
        }

        int totalOverdueHours = 0;

        foreach (var task in GetTasks())
        {
            if (task.Epic != null && task.Epic.Equals(epic) && task.UserTrashpaperId == null)
            {
                totalOverdueHours += Math.Max(task.InvestedTime - task.EstimatedTime, 0);
            }
        }

        return totalOverdueHours;
    }

    public List<PanelTask> GetAccessibleTasks(User user)
    {
        List<PanelTask> visibleTasks = new List<PanelTask>();
        const int OUTDATED_PANEL_ID = 1;
        foreach (var panel in GetPanels())
        {
            if(panel.Id == OUTDATED_PANEL_ID) continue;
            if(panel.Team.TeamUsersList.Any(x => x.Id == user.Id) || user.Type == TypeUser.Admin)
            {
                foreach (var task in panel.PanelTaskList)
                {
                    visibleTasks.Add(task);
                }
            }
        }

        return visibleTasks;
    }

    public bool EpicHasTasks(Epic epic)
    {
        foreach (var task in GetTasks())
        {
            if (task.Epic == epic && task.UserTrashpaperId == null)
            {
                return true;
            }
        }
        return false;
    }
    
    public bool EpicHasCompleteTasks(Epic epic)
    {
        foreach (var task in GetTasks())
        {
            if (task.Epic == epic && task.State == StateTask.Finished && task.UserTrashpaperId == null)
            {
                return true;
            }
        }
        return false;
    }
    
    public bool EpicHasIncompleteTasks(Epic epic)
    {
        foreach (var task in GetTasks())
        {
            if (task.Epic == epic && task.State == StateTask.Unfinished && task.UserTrashpaperId == null)
            {
                return true;
            }
        }
        return false;
    }

}