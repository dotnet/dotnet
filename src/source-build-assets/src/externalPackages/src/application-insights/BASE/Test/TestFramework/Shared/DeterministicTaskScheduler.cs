namespace Microsoft.ApplicationInsights.TestFramework
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// TaskScheduler for executing tasks on the same thread that calls RunTasksUntilIdle() or RunPendingTasks().
    /// </summary>
    public class DeterministicTaskScheduler : TaskScheduler
    {
        private List<Task> scheduledTasks;

        public DeterministicTaskScheduler(): base()
        {
            scheduledTasks = new List<Task>();
        }

        protected override void QueueTask(Task task)
        {
            this.scheduledTasks.Add(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return base.TryExecuteTask(task);
        }

        protected override IEnumerable<Task> GetScheduledTasks() => this.scheduledTasks;

        public override int MaximumConcurrencyLevel => 1;

        public IEnumerable<Task> ScheduledTasks => this.scheduledTasks;

        /// <summary>
        /// Executes the scheduled Tasks synchronously on the current thread. If those tasks schedule new tasks
        /// they will also be executed until no pending tasks are left.
        /// </summary>
        public void RunTasksUntilIdle()
        {
            while (this.scheduledTasks.Any())
            {
                this.RunPendingTasks();
            }
        }

        /// <summary>
        /// Executes the scheduled Tasks synchronously on the current thread. If those tasks schedule new tasks
        /// they will only be executed with the next call to RunTasksUntilIdle() or RunPendingTasks(). 
        /// </summary>
        public void RunPendingTasks()
        {
            foreach (var task in this.scheduledTasks.ToArray())
            {
                base.TryExecuteTask(task);
                this.scheduledTasks.Remove(task);
            }
        }
    }
}
