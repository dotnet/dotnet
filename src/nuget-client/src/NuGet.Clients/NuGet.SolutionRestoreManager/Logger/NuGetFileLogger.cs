// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if DEBUG

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using NuGet.Common;

namespace NuGet.SolutionRestoreManager
{
    internal sealed class NuGetFileLogger : IDisposable
    {
        private bool _isDisposed;
        private readonly Lazy<StreamWriter> _streamWriter;
        private readonly string _logDirectoryPath;
        private readonly DateTimeOffset _startTime;
        private readonly Stopwatch _stopwatch;
        private readonly object _streamWriterLock;

        internal static NuGetFileLogger DefaultInstance { get; } = new NuGetFileLogger(EnvironmentVariableWrapper.Instance);

        public bool IsEnabled { get; }

        public bool ShouldFormatWithTime { get; }

        // The DateTimeOffset and Stopwatch ticks are not equivalent. 1/10000000 is 1 DateTime tick.
        public DateTimeOffset Now => _startTime.AddTicks(_stopwatch.ElapsedTicks * 10000000 / Stopwatch.Frequency);

        internal NuGetFileLogger(IEnvironmentVariableReader environmentVariableReader)
        {
            if (environmentVariableReader == null)
            {
                throw new ArgumentNullException(nameof(environmentVariableReader));
            }

            _logDirectoryPath = environmentVariableReader.GetEnvironmentVariable("NUGET_VS_RESTORE_LOGGING_PATH");

            if (!string.IsNullOrWhiteSpace(_logDirectoryPath))
            {
                IsEnabled = true;
            }

            var formatWithTime = environmentVariableReader.GetEnvironmentVariable("NUGET_VS_RESTORE_FORMAT_WITH_TIME");

            if (!string.IsNullOrWhiteSpace(formatWithTime))
            {
                _ = bool.TryParse(formatWithTime, out bool formatWithTimeOverride);

                ShouldFormatWithTime = formatWithTimeOverride;
            }

            _startTime = DateTimeOffset.UtcNow;
            _stopwatch = Stopwatch.StartNew();

            // Created outside of the lambda below to capture the current time.
            var message = $"The stopwatch frequency is {Stopwatch.Frequency}";

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

        public void Write(string logMessage)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(NuGetFileLogger));
            }

            if (logMessage == null)
            {
                throw new ArgumentNullException(nameof(logMessage));
            }

            lock (_streamWriterLock)
            {
                string message = logMessage;
                if (ShouldFormatWithTime)
                {
                    message = FormatWithTime(logMessage);
                }
                _streamWriter.Value.WriteLine(message);

            }
        }

        private string FormatWithTime(string logMessage)
        {
            return Now.ToString("O", CultureInfo.CurrentCulture) + " MI:" + Thread.CurrentThread.ManagedThreadId + " : " + logMessage;
        }

        private StreamWriter CreateStreamWriter(string message)
        {
            if (IsEnabled)
            {
                var fileName = $"NuGet_Log_{DateTime.UtcNow.Ticks:x}.log";
                var filePath = Path.Combine(_logDirectoryPath, fileName);
                var stream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.Read);

                try
                {
                    var streamWriter = new StreamWriter(stream);

                    streamWriter.AutoFlush = true;

                    streamWriter.WriteLine(FormatWithTime(message));

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
#endif
