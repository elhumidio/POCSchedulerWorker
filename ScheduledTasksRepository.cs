using POCSchedulerWorker;

public class ScheduledTasksRepository
{
    private readonly ApplicationDbContext _context;

    public ScheduledTasksRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<ScheduledTask> GetScheduledTasks()
    {

        var scheduledTasks = _context.ScheduledTasks.ToList();
        return scheduledTasks;

    }
}
