namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.SelfDiagnostics
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// SelfDiagnosticsConfigRefresher class checks a location for a configuration file
    /// and open a MemoryMappedFile of a configured size at the configured file path.
    /// The class provides a stream object with proper write position if the configuration
    /// file is present and valid. Otherwise, the stream object would be unavailable,
    /// nothing will be logged to any file.
    /// </summary>
    internal class SelfDiagnosticsConfigRefresher : IDisposable
    {
        private const int ConfigurationUpdatePeriodMilliSeconds = 10000;

        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly Task worker;
        private readonly SelfDiagnosticsConfigParser configParser;
        private readonly MemoryMappedFileHandler memoryMappedFileHandler;

        private bool disposedValue;

        // Once the configuration file is valid, an eventListener object will be created.
        private SelfDiagnosticsEventListener eventListener;

        private EventLevel logEventLevel = (EventLevel)(-1);

        public SelfDiagnosticsConfigRefresher()
        {
            this.configParser = new SelfDiagnosticsConfigParser();
            this.memoryMappedFileHandler = new MemoryMappedFileHandler();
            this.UpdateMemoryMappedFileFromConfiguration();
            this.cancellationTokenSource = new CancellationTokenSource();
            this.worker = Task.Run(() => this.Worker(this.cancellationTokenSource.Token), this.cancellationTokenSource.Token);
        }

        public string CurrentFilePath => this.memoryMappedFileHandler.CurrentFilePath;

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private async Task Worker(CancellationToken cancellationToken)
        {
            await Task.Delay(ConfigurationUpdatePeriodMilliSeconds, cancellationToken).ConfigureAwait(false);
            while (!cancellationToken.IsCancellationRequested)
            {
                this.UpdateMemoryMappedFileFromConfiguration();
                await Task.Delay(ConfigurationUpdatePeriodMilliSeconds, cancellationToken).ConfigureAwait(false);
            }
        }

        private void UpdateMemoryMappedFileFromConfiguration()
        {
            if (this.configParser.TryGetConfiguration(out string newLogDirectory, out int fileSizeInKB, out EventLevel newEventLevel))
            {
                int newFileSize = fileSizeInKB * 1024;
                if (!newLogDirectory.Equals(this.memoryMappedFileHandler.LogDirectory, StringComparison.Ordinal) || this.memoryMappedFileHandler.LogFileSize != newFileSize)
                {
                    this.memoryMappedFileHandler.CloseLogFile();
                    this.memoryMappedFileHandler.CreateLogFile(newLogDirectory, newFileSize);
                }

                if (!newEventLevel.Equals(this.logEventLevel))
                {
                    if (this.eventListener != null)
                    {
                        this.eventListener.Dispose();
                    }

                    this.eventListener = new SelfDiagnosticsEventListener(newEventLevel, this.memoryMappedFileHandler);
                    this.logEventLevel = newEventLevel;
                }
            }
            else
            {
                this.memoryMappedFileHandler.CloseLogFile();
            }
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.cancellationTokenSource.Cancel(false);
                    try
                    {
                        this.worker.Wait();
                    }
                    catch (AggregateException)
                    {
                    }
                    finally
                    {
                        this.cancellationTokenSource.Dispose();
                    }

                    // Ensure worker thread properly finishes.
                    // Or it might have created another MemoryMappedFile in that thread
                    // after the Dispose() below is called.
                    this.memoryMappedFileHandler.Dispose();
                    if (this.eventListener != null)
                    {
                        this.eventListener.Dispose();
                    }
                }

                this.disposedValue = true;
            }
        }
    }
}
