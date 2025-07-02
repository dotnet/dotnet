namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Runs tasks synchronously, on the current thread. 
    /// From <a href="http://code.msdn.microsoft.com/Samples-for-Parallel-b4b76364/view/SourceCode"/>.
    /// </summary>
    internal sealed class CurrentThreadTaskScheduler : TaskScheduler
    {
        public static readonly TaskScheduler Instance = new CurrentThreadTaskScheduler();

        public override int MaximumConcurrencyLevel 
        { 
            get { return 1; } 
        }

        protected override void QueueTask(Task task)
        {
            this.TryExecuteTask(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return this.TryExecuteTask(task);
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return Enumerable.Empty<Task>();
        }
    }
}
