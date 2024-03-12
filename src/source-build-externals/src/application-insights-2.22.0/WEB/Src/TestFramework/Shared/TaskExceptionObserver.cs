namespace Microsoft.ApplicationInsights.Web.TestFramework
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// When disposed, throws unobserved task exceptions.
    /// </summary>
    internal sealed class TaskExceptionObserver : IDisposable
    {
        private static readonly MethodInfo GetScheduledTasksMethod = typeof(TaskScheduler).GetMethod("GetScheduledTasks", BindingFlags.Instance | BindingFlags.NonPublic);
        private List<AggregateException> unobservedExceptions = new List<AggregateException>();

        public TaskExceptionObserver()
        {
            TaskScheduler.UnobservedTaskException += this.HandleUnobservedTaskExceptionEvent;
        }

        public void Dispose()
        {
            WaitForCurrentTasksToFinish();
            MakeTaskSchedulerRaiseUnobservedTaskExceptionEvent();
            TaskScheduler.UnobservedTaskException -= this.HandleUnobservedTaskExceptionEvent;

            if (this.unobservedExceptions.Count > 0)
            {
                throw this.unobservedExceptions[0];
            }
        }

        private static void WaitForCurrentTasksToFinish()
        {
            IEnumerable<Task> scheduledTasks;
            do
            {
                scheduledTasks = (IEnumerable<Task>)TaskExceptionObserver.GetScheduledTasksMethod.Invoke(TaskScheduler.Current, null);
                Task.WaitAll(scheduledTasks.ToArray());
            }
            while (scheduledTasks.Any());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.GC.Collect", Justification = "This is forcing a test scenario")]
        private static void MakeTaskSchedulerRaiseUnobservedTaskExceptionEvent()
        {
            GC.Collect(2, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
        }

        private void HandleUnobservedTaskExceptionEvent(object sender, UnobservedTaskExceptionEventArgs e)
        {
            this.unobservedExceptions.Add(e.Exception);
        }
    }
}
