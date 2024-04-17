namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers
{
    using System;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    
    internal class StubApplicationLifecycle : IApplicationLifecycle
    {
        public event Action<object, object> Started;

        public event EventHandler<ApplicationStoppingEventArgs> Stopping;

        public Action<object, object> StartedHandler 
        { 
            get { return this.Started; } 
        }

        public EventHandler<ApplicationStoppingEventArgs> StoppingHandler 
        { 
            get { return this.Stopping; } 
        }

        public void OnStopping(ApplicationStoppingEventArgs e)
        {
            EventHandler<ApplicationStoppingEventArgs> handler = this.Stopping;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void OnStarted(EventArgs e)
        {
            Action<object, object> handler = this.Started;
            if (handler != null)
            {
                handler(this, e);
            }
        }       
    }
}
