// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using NuGet.Common;

namespace NuGet.Protocol.Plugins
{
    internal sealed class PluginLogger : IPluginLogger
    {
        private bool _isDisposed;
        private readonly Lazy<StreamWriter> _streamWriter;
        private readonly string? _logDirectoryPath;
        private readonly DateTimeOffset _startTime;
        private readonly Stopwatch _stopwatch;
        private readonly object _streamWriterLock;

        static PluginLogger()
        {
            // The log file is a live OS resource written to by the plugins, so it is discarded together with them at
            // the end of the build rather than at the start of each restore. A plugin outlives the restore that
            // created it - a build commonly restores several times - and closing the log underneath it would leave it
            // holding a disposed logger.
            StaticState.BuildEnded += ResetDefaultInstance;
        }

        private static readonly object s_defaultInstanceLock = new object();
        private static PluginLogger? s_defaultInstance;

        /// <summary>
        /// The process-wide logger, created on first use after each reset so that a process reused across builds
        /// reads <c>NUGET_PLUGIN_ENABLE_LOG</c> and <c>NUGET_PLUGIN_LOG_DIRECTORY_PATH</c> from the environment of the
        /// build that actually uses it.
        /// </summary>
        internal static PluginLogger DefaultInstance
        {
            get
            {
                lock (s_defaultInstanceLock)
                {
                    return s_defaultInstance ??= new PluginLogger(EnvironmentVariableWrapper.Instance);
                }
            }
        }

        /// <summary>
        /// Discards <see cref="DefaultInstance" />, closing its log file, so that the next build builds a new one from
        /// its own environment. Subscribed to <see cref="StaticState.BuildEnded" />, which runs once the
        /// plugins that write to the log are being torn down.
        /// </summary>
        internal static void ResetDefaultInstance()
        {
            PluginLogger? previous;

            lock (s_defaultInstanceLock)
            {
                previous = s_defaultInstance;
                s_defaultInstance = null;
            }

            previous?.Dispose();
        }

        public bool IsEnabled { get; }
        // The DateTimeOffset and Stopwatch ticks are not equivalent. 1/10000000 is 1 DateTime tick.
        public DateTimeOffset Now => _startTime.AddTicks(_stopwatch.ElapsedTicks * 10000000 / Stopwatch.Frequency);

        internal PluginLogger(IEnvironmentVariableReader environmentVariableReader)
        {
            if (environmentVariableReader == null)
            {
                throw new ArgumentNullException(nameof(environmentVariableReader));
            }

            var value = environmentVariableReader.GetEnvironmentVariable(EnvironmentVariableConstants.EnableLog);

            IsEnabled = bool.TryParse(value, out var enable) && enable;

            if (IsEnabled)
            {
                _logDirectoryPath = environmentVariableReader.GetEnvironmentVariable(EnvironmentVariableConstants.LogDirectoryPath);

                if (string.IsNullOrWhiteSpace(_logDirectoryPath))
                {
                    _logDirectoryPath = Environment.CurrentDirectory;
                }
            }

            _startTime = DateTimeOffset.UtcNow;
            _stopwatch = Stopwatch.StartNew();

            // Created outside of the lambda below to capture the current time.
            var message = new StopwatchLogMessage(Now, Stopwatch.Frequency);

            _streamWriter = new Lazy<StreamWriter>(() => CreateStreamWriter(message));
            _streamWriterLock = new object();
        }

        public void Dispose()
        {
            lock (_streamWriterLock)
            {
                if (_isDisposed)
                {
                    return;
                }

                if (_streamWriter.IsValueCreated)
                {
                    _streamWriter.Value.Dispose();
                }

                _isDisposed = true;
            }

            GC.SuppressFinalize(this);
        }

        public void Write(IPluginLogMessage message)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (message == null)
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(message));
            }

            lock (_streamWriterLock)
            {
                if (_isDisposed)
                {
                    // A plugin can outlive the logger it captured, and teardown itself logs, so writes race with
                    // disposal. Diagnostics must never fail the operation being diagnosed, so drop the message rather
                    // than writing to a closed stream. Checked under the same lock that disposal takes, so a write can
                    // never slip past this and reach a disposed StreamWriter.
                    return;
                }

                _streamWriter.Value.WriteLine(message.ToString());
            }
        }

        private StreamWriter CreateStreamWriter(IPluginLogMessage message)
        {
            if (IsEnabled)
            {
                string logDirectoryPath = _logDirectoryPath
                    ?? throw new InvalidOperationException("Log directory must be set when plugin logging is enabled.");
                FileInfo file;
                int processId;

                using (var process = Process.GetCurrentProcess())
                {
                    string processFileName = process.MainModule?.FileName
                        ?? throw new InvalidOperationException("The current process must have a main module file name for plugin logging.");
                    file = new FileInfo(processFileName);
                    processId = process.Id;
                }

                var fileName = $"NuGet_PluginLogFor_{Path.GetFileNameWithoutExtension(file.Name)}_{DateTime.UtcNow.Ticks:x}_{processId}.log";
                var filePath = Path.Combine(logDirectoryPath, fileName);
                var stream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.Read);

                try
                {
                    var streamWriter = new StreamWriter(stream);

                    streamWriter.AutoFlush = true;

                    streamWriter.WriteLine(message.ToString());

                    return streamWriter;
                }
                catch (Exception)
                {
                    stream.Dispose();

                    throw;
                }
            }

            return StreamWriter.Null;
        }
    }
}
