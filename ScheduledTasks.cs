using Cronos;
using System.ComponentModel.DataAnnotations;

namespace POCSchedulerWorker
{
    public class ScheduledTask
    {
        [Key]
        public int Id { get; set; }

        public string Task { get; set; }

        public string EndpointUrl { get; set; }

        public string CronExpression { get; set; }
    }
    public class ScheduledTaskSchedule
    {
        public ScheduledTask Task { get; set; }
        public CronExpression CronSchedule { get; set; }
        public DateTime? NextRun { get; set; }
    }

}