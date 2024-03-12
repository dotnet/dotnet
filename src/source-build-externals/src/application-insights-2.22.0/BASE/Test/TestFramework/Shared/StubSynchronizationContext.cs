namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    using System.Threading;

    internal class StubSynchronizationContext : SynchronizationContext, IDisposable
    {
        private readonly SynchronizationContext originalContext;

        public StubSynchronizationContext()
        {
            this.OnPost = base.Post;

            this.originalContext = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(this);
        }

        public Action<SendOrPostCallback, object> OnPost { get; set; }

        public override void Post(SendOrPostCallback callback, object state)
        {
            this.OnPost(callback, state);
        }

        // For convenience of using (StubSynchronizationContext)
        void IDisposable.Dispose()
        {
            SynchronizationContext.SetSynchronizationContext(this.originalContext);
        }
    }
}
