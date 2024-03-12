namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Encapsulates arguments of the <see cref="IApplicationLifecycle.Stopping"/> event.
    /// </summary>
    public class ApplicationStoppingEventArgs : EventArgs
    {
        internal static new readonly ApplicationStoppingEventArgs Empty = new ApplicationStoppingEventArgs(asyncMethod => asyncMethod());

        private readonly Func<Func<Task>, Task> asyncMethodRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStoppingEventArgs"/> class with the specified runner of asynchronous methods.
        /// </summary>
        public ApplicationStoppingEventArgs(Func<Func<Task>, Task> asyncMethodRunner)
        {
            this.asyncMethodRunner = asyncMethodRunner ?? throw new ArgumentNullException(nameof(asyncMethodRunner));
        }

        /// <summary>
        /// Runs the specified asynchronous method while preventing the application from exiting.
        /// </summary>
        public async void Run(Func<Task> asyncMethod)
        {
            try
            {
                await this.asyncMethodRunner(asyncMethod).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                TelemetryChannelEventSource.Log.UnexpectedExceptionInStopError(exception.ToString());
            }            
        }
    }
}
