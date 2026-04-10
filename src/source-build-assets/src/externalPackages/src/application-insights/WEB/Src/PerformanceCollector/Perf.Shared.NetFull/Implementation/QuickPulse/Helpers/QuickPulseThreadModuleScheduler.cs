namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers
{
    using System;
    using System.Threading;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    internal class QuickPulseThreadModuleScheduler : IQuickPulseModuleScheduler
    {
        public static readonly QuickPulseThreadModuleScheduler Instance = new QuickPulseThreadModuleScheduler();

        private QuickPulseThreadModuleScheduler()
        {
        }

        public IQuickPulseModuleSchedulerHandle Execute(Action<CancellationToken> action)
        {
            State state = new State(action);

            return state;
        }

        private class State : IQuickPulseModuleSchedulerHandle
        {
            private readonly Thread thread;

            private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            public State(Action<CancellationToken> action)
            {
                this.thread = new Thread(this.Worker) { IsBackground = true };
                this.thread.Start(action);
            }

            public void Stop(bool wait)
            {
                this.Dispose();
                if (wait)
                {
                    this.thread.Join();
                }
            }

            public void Dispose()
            {
                this.cancellationTokenSource.Cancel();
                this.cancellationTokenSource.Dispose();
            }

            private void Worker(object state)
            {
                try
                {
                    (state as Action<CancellationToken>)?.Invoke(this.cancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    // This is a Thread, and we don't want any exception thrown ever from this part as this would cause application crash.
                    try
                    {
                        QuickPulseEventSource.Log.UnknownErrorEvent(ex.ToInvariantString());
                    }
                    catch (Exception)
                    {
                        // Intentionally empty. If EventSource writing itself is failing as well, there is nothing more to be done here.
                        // The best that can be done is atleast prevent application crash due to unhandledexception from Thread.
                    }
                }
            }
        }
    }
}
