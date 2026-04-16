    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.Diagnostics.Tracing;
    using Microsoft.Diagnostics.Tracing.Session;

    /// <summary>
    /// This utility will handle the management of <see cref="TraceEventSession" /> to subscribe to ETW events.
    /// This will create two TraceEventSessions, one to write to an *.etl file and a second to output to console in real-time.
    /// </summary>
    /// <remarks>
    /// <see href="https://github.com/microsoft/perfview/blob/master/documentation/TraceEvent/TraceEventProgrammersGuide.md" />
    /// <see href="https://github.com/microsoft/perfview/blob/master/src/TraceEvent/Samples/22_ObserveEventSource.cs" />
    /// </remarks>
    internal class TraceEventUtility : IDisposable
    {
        private const string TraceFileSessionName = "ApplicationInsights_etlFile_TraceSession";
        private const string TraceConsoleSessionName = "ApplicationInsights_Console_TraceSession";

        private readonly bool shouldLogToFile;
        private readonly bool shouldLogToConsole;
        private readonly string logDirectory;

        private TraceEventSession fileSession;
        private TraceEventSession consoleSession;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceEventUtility"/> class.
        /// </summary>
        /// <param name="shouldLogToFile">Enable or disable logging to an ETL file.</param>
        /// <param name="shouldLogToConsole">Enable or disable logging to console.</param>
        /// <param name="logDirectory">Directory to save ETL file. If not provided, will use Powershell directory.</param>
        public TraceEventUtility(bool shouldLogToFile = true, bool shouldLogToConsole = true, string logDirectory = null)
        {
            this.shouldLogToConsole = shouldLogToConsole;
            this.shouldLogToFile = shouldLogToFile;
            this.logDirectory = logDirectory;
        }

        /// <summary>
        /// Create TraceEventSession and begin logging ETW events.
        /// </summary>
        public void Start()
        {
            if (this.shouldLogToFile)
            {
                // https://blogs.msdn.microsoft.com/vancem/2012/12/20/using-tracesource-to-log-etw-data-to-a-file/
                this.fileSession = new TraceEventSession(sessionName: TraceFileSessionName, fileName: this.GetFilePath())
                {
                    StopOnDispose = true
                };
                EnableProviders(this.fileSession);
            }

            if (this.shouldLogToConsole)
            {
                // https://blogs.msdn.microsoft.com/vancem/2012/12/20/an-end-to-end-etw-tracing-example-eventsource-and-traceevent/
                this.consoleSession = new TraceEventSession(sessionName: TraceConsoleSessionName)
                {
                    StopOnDispose = true
                };
                EnableProviders(this.consoleSession);
                this.consoleSession.Source.Dynamic.All += this.ConsoleEventHandler;
                Task.Run(() => this.consoleSession.Source.Process()); // Source.Process() will block the thread, so execute on a new thread.
            }
        }

        /// <summary>
        /// Find sessions if they exist and stop them.
        /// </summary>
        public void Stop()
        {
            this.consoleSession?.Stop();
            this.fileSession?.Stop();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Stop();
            this.consoleSession?.Dispose();
            this.fileSession?.Dispose();
        }

        /// <summary>
        /// Subscribe to these providers.
        /// </summary>
        /// <param name="session">Session to enable logging.</param>
        /// <remarks>
        /// These identifiers come from the EventSource attribute on an EventSource class.
        /// Ex: [EventSource(Name = "Microsoft-Demos-MySource", Guid = "833f567a-7254-4392-a89d-29b7f7fa354d")]
        /// If the Guid exists in the attribute, you MUST use that Guid.
        /// Otherwise, use the name.
        /// </remarks>
        private static void EnableProviders(TraceEventSession session)
        {
#if DEBUG
            // Developer sample app used to generate logs for testing.
            session.EnableProvider(providerName: "Microsoft-Demos-MySource");
#endif

            // Microsoft-ApplicationInsights-Redfield-Configurator
            session.EnableProvider(providerGuid: Guid.Parse("090fc833-b744-4805-a6dd-4cb0b840a11f"));

            // Microsoft-ApplicationInsights-Redfield-VmExtensionHandler
            session.EnableProvider(providerGuid: Guid.Parse("7014a441-75d7-444f-b1c6-4b2ec9b06f20"));

            // Microsoft-ApplicationInsights-IIS-ManagedHttpModuleHelper
            session.EnableProvider(providerGuid: Guid.Parse("61f6ca3b-4b5f-5602-fa60-759a2a2d1fbd"));

            // Microsoft-ApplicationInsights-FrameworkLightup
            session.EnableProvider(providerGuid: Guid.Parse("323adc25-e39b-5c87-8658-2c1af1a92dc5"));

            // Microsoft-ApplicationInsights-RedfieldIISModule
            session.EnableProvider(providerGuid: Guid.Parse("252e28f4-43f9-5771-197a-e8c7e750a984"));
        }

        /// <summary>
        /// Event handler for the Console session.
        /// Manifest data is not relevant to the life console so we filter out these messages.
        /// </summary>
        /// <param name="data">Data received to be output to the console.</param>
        private void ConsoleEventHandler(TraceEvent data)
        {
            if (data.EventName != "ManifestData")
            {
                // this runs on a separate thread so cannot use the cmdlet logger (ui thread).
                Console.WriteLine($"{data.TimeStamp.ToLongTimeString()} EVENT: {data.ProviderName} {data.EventName} {data.FormattedMessage ?? string.Empty}");
            }
        }

        private string GetFilePath()
        {
            // TODO: If implement sdk/redfield toggle, should update this name.
            string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
            fileName += "_ApplicationInsights_ETW_Trace.etl";

            string directory = this.logDirectory ?? Path.Combine(new PowerShellRuntimePaths().ParentDirectory, "logs");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var filePath = Path.Combine(directory, fileName);
            Console.WriteLine($"Log File: {filePath}");
            return filePath;
        }
    }
