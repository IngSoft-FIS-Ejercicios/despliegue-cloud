using BlazorBootstrap;
using Domain;
namespace Logic;

public class DataGraphicReportTasks
{
    public List<string> labels = new List<string>();
    public List<IChartDataset> datasets = new List<IChartDataset>();
}
public class GraphicReport
{
    private PanelLogic _panelLogic;


    public GraphicReport(PanelLogic panelLogic)
    {
        _panelLogic = panelLogic;
    }
    
    public DataGraphicReportTasks GetUnfinishTasks(Epic selectedEpic)
    {
        DataGraphicReportTasks data = new DataGraphicReportTasks();
        List<double?> investedHours = new List<double?>();
        List<double?> hoursLeft = new List<double?>();
        List<double?> overdueHours = new List<double?>();
        
        foreach (var task in _panelLogic.GetTasks())
        {
            if (task.Epic == selectedEpic && task.State == StateTask.Unfinished && task.UserTrashpaperId == null)
            {
                data.labels.Add(task.Title);
                investedHours.Add(System.Math.Min(task.InvestedTime, task.EstimatedTime));
                if (task.EstimatedTime - task.InvestedTime > 0)
                {
                    hoursLeft.Add(task.EstimatedTime - task.InvestedTime);
                    overdueHours.Add(0);
                }
                else
                {
                    hoursLeft.Add(0);
                    overdueHours.Add(-1 * (task.EstimatedTime - task.InvestedTime));
                }
            }
        }
        
        data.datasets.Add(createDataset("Invested Hours",investedHours,"#A05273"));
        data.datasets.Add(createDataset("Overdue Hours", overdueHours, "#6B354C"));
        data.datasets.Add(createDataset("Hours Left", hoursLeft, "#F5B7A9"));

        return data;
    }

    public DataGraphicReportTasks GetFinishTasks(Epic selectedEpic)
    {
        DataGraphicReportTasks data = new DataGraphicReportTasks();

        List<double?> finishedInvestedHoursOk = new List<double?>();
        List<double?> finishedInvestedHoursOverestimated = new List<double?>();
        List<double?> finishedHoursLeftOverestimated = new List<double?>();
        List<double?> finishedInvestedHoursUnderestimated = new List<double?>();
        List<double?> finishedOverdueHoursUnderestimated = new List<double?>();
        
        foreach (var task in _panelLogic.GetTasks())
        {
            if (task.Epic == selectedEpic && task.State == StateTask.Finished && task.UserTrashpaperId == null)
            {
                if (task.CompareEstimatedInvested() == EstimateComparison.Ok)
                {
                    data.labels.Add(task.Title + " (OK)");
                    finishedInvestedHoursOk.Add(task.InvestedTime);
                    finishedInvestedHoursOverestimated.Add(0);
                    finishedHoursLeftOverestimated.Add(0);
                    finishedInvestedHoursUnderestimated.Add(0);
                    finishedOverdueHoursUnderestimated.Add(0);
                } 
                else if (task.CompareEstimatedInvested() == EstimateComparison.Overestimated)
                {
                    data.labels.Add(task.Title+ " (Overestimated)");
                    finishedInvestedHoursOverestimated.Add(task.InvestedTime);
                    finishedHoursLeftOverestimated.Add(task.EstimatedTime - task.InvestedTime);
                    finishedInvestedHoursOk.Add(0);
                    finishedInvestedHoursUnderestimated.Add(0);
                    finishedOverdueHoursUnderestimated.Add(0);
                }
                else
                {
                    data.labels.Add(task.Title+ " (Underestimated)");
                    finishedInvestedHoursUnderestimated.Add(task.EstimatedTime);
                    finishedOverdueHoursUnderestimated.Add(-1 * (task.EstimatedTime - task.InvestedTime));
                    finishedInvestedHoursOk.Add(0);
                    finishedInvestedHoursOverestimated.Add(0);
                    finishedHoursLeftOverestimated.Add(0);
                }
                
            }
        }

        data.datasets.Add(createDataset("Invested Hours", finishedInvestedHoursOk, "#4CAF50"));
        data.datasets.Add(createDataset("Invested Hours", finishedInvestedHoursUnderestimated, "#F44336"));
        data.datasets.Add(createDataset("Overdue Hours",finishedOverdueHoursUnderestimated,"#B71C1C"));
        data.datasets.Add(createDataset("Invested Hours", finishedInvestedHoursOverestimated, "#03A9F4"));
        data.datasets.Add(createDataset("Expected Hours Left", finishedHoursLeftOverestimated, "#81D4FA"));

        return data;
    }

    private BarChartDataset createDataset(string name, List<double?> infoList, string color )
    {
        BarChartDataset dataset = new BarChartDataset()
        {
            Label = name,
            Data = infoList,
            BackgroundColor = new List<string> { color  },
        };
        return dataset;
    }
    
}