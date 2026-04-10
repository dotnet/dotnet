namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal class QuickPulseTaskModuleScheduler : IQuickPulseModuleScheduler
    {
        public static readonly QuickPulseTaskModuleScheduler Instance = new QuickPulseTaskModuleScheduler();

        private QuickPulseTaskModuleScheduler()
        {
        }

        public IQuickPulseModuleSchedulerHandle Execute(Action<CancellationToken> action)
        {
            State state = new State(action);

            return state;
        }

        private class State : IQuickPulseModuleSchedulerHandle
        {
            private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            private readonly Task task;

            public State(Action<CancellationToken> action)
            {
                this.task = Task.Factory.StartNew(
                    action: () =>
                    {
                        action?.Invoke(this.cancellationTokenSource.Token);
                    }, 
                    creationOptions: TaskCreationOptions.LongRunning);
            }

            public void Stop(bool wait)
            {
                this.Dispose();
                if (wait)
                {
                    // wait and ignore all exceptions
                    this.task.ContinueWith(_ => { }).Wait();
                }
            }

            public void Dispose()
            {
                this.cancellationTokenSource.Cancel(throwOnFirstException: false);
                this.cancellationTokenSource.Dispose();
            }
        }
    }
}
