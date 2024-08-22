using Cronos;
using Microsoft.EntityFrameworkCore;
using POCSchedulerWorker;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer("Data Source=mssql-brandturijobs.live.stepstone.tools;Initial Catalog=Turijobs.master;User ID=tjweb;Password=g32qmYUId;TrustServerCertificate=True");

        var cronSchedules = await GetScheduledTasksAsync(optionsBuilder.Options);

        while (true)
        {
            var now = DateTime.UtcNow;

            foreach (var schedule in cronSchedules)
            {
                using var httpClient = new HttpClient();
                using var context = new ApplicationDbContext(optionsBuilder.Options);

                if (schedule.NextRun.HasValue && now > schedule.NextRun.Value)
                {
                    await CallEndpointAsync(httpClient, schedule.Task.EndpointUrl);
                    schedule.NextRun = schedule.CronSchedule.GetNextOccurrence(now);
                }
            }

            await Task.Delay(1000);
        }
    }

    private static async Task<List<ScheduledTaskSchedule>> GetScheduledTasksAsync(DbContextOptions<ApplicationDbContext> optionsBuilder)
    {
        using var context = new ApplicationDbContext(optionsBuilder);
        var repository = new ScheduledTasksRepository(context);
        var tasks = repository.GetScheduledTasks();

        return tasks.Select(t => new ScheduledTaskSchedule
        {
            Task = t,
            CronSchedule = CronExpression.Parse(t.CronExpression),
            NextRun = CronExpression.Parse(t.CronExpression).GetNextOccurrence(DateTime.UtcNow, TimeZoneInfo.Local, true)
        }).ToList();
    }

    private static async Task CallEndpointAsync(HttpClient httpClient, string url)
    {
        try
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al llamar al endpoint {url}: {ex.Message}");
        }
    }
}