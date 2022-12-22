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
        private readonly string _logDirectoryPath;
        private readonly DateTimeOffset _startTime;
        private readonly Stopwatch _stopwatch;
        private readonly object _streamWriterLock;

        internal static PluginLogger DefaultInstance { get; } = new PluginLogger(EnvironmentVariableWrapper.Instance);

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
            if (!_isDisposed)
            {
                if (_streamWriter.IsValueCreated)
                {
                    _streamWriter.Value.Dispose();
                }

                GC.SuppressFinalize(this);
                _isDisposed = true;
            }
        }

        public void Write(IPluginLogMessage message)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(PluginLogger));
            }

            if (message == null)
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(message));
            }

            lock (_streamWriterLock)
            {
                _streamWriter.Value.WriteLine(message.ToString());
            }
        }

        private StreamWriter CreateStreamWriter(IPluginLogMessage message)
        {
            if (IsEnabled)
            {
                FileInfo file;
                int processId;

                using (var process = Process.GetCurrentProcess())
                {
                    file = new FileInfo(process.MainModule.FileName);
                    processId = process.Id;
                }

                var fileName = $"NuGet_PluginLogFor_{Path.GetFileNameWithoutExtension(file.Name)}_{DateTime.UtcNow.Ticks:x}_{Process.GetCurrentProcess().Id}.log";
                var filePath = Path.Combine(_logDirectoryPath, fileName);
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
