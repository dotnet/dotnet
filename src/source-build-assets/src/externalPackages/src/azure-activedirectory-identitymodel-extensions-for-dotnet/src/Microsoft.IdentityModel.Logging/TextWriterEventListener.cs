// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.Tracing;
using System.IO;
using Microsoft.IdentityModel.Abstractions;

namespace Microsoft.IdentityModel.Logging
{
    /// <summary>
    /// Event listener that writes logs to a file or a fileStream provided by user.
    /// </summary>
    public class TextWriterEventListener : EventListener
    {
        private StreamWriter _streamWriter;
        private bool _disposeStreamWriter = true;

        /// <summary>
        /// Name of the default log file, excluding its path.
        /// </summary>
        public readonly static string DefaultLogFileName = "IdentityModelLogs.txt";

        /// <summary>
        /// Initializes a new instance of <see cref="TextWriterEventListener"/> that writes logs to text file.
        /// </summary>
        public TextWriterEventListener()
        {
            try
            {
                Stream fileStream = new FileStream(DefaultLogFileName, FileMode.OpenOrCreate, FileAccess.Write);
                _streamWriter = new StreamWriter(fileStream);
                _streamWriter.AutoFlush = true;
            }
            catch (Exception ex) when (LogHelper.IsEnabled(EventLogLevel.Error))
            {
                LogHelper.LogExceptionMessage(new InvalidOperationException(LogMessages.MIML10001, ex));
                throw;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TextWriterEventListener"/> that writes logs to text file.
        /// </summary>
        /// <param name="filePath">location of the file where log messages will be written.</param>
        public TextWriterEventListener(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw LogHelper.LogArgumentNullException(nameof(filePath));

            try
            {
                Stream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                _streamWriter = new StreamWriter(fileStream);
                _streamWriter.AutoFlush = true;
            }
            catch (Exception ex) when (LogHelper.IsEnabled(EventLogLevel.Error))
            {
                LogHelper.LogExceptionMessage(new InvalidOperationException(LogMessages.MIML10001, ex));
                throw;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TextWriterEventListener"/> that writes logs to text file.
        /// </summary>
        /// <param name="streamWriter">StreamWriter where logs will be written.</param>
        public TextWriterEventListener(StreamWriter streamWriter)
        {
            if (streamWriter == null)
                throw LogHelper.LogArgumentNullException("streamWriter");

            _streamWriter = streamWriter;
            _disposeStreamWriter = false;
        }

        /// <summary>
        /// Called whenever an event has been written by an event source for which the event listener has enabled events.
        /// </summary>
        /// <param name="eventData"><see cref="EventWrittenEventArgs"/></param>
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (eventData == null)
                throw LogHelper.LogArgumentNullException("eventData");

            if (eventData.Payload == null || eventData.Payload.Count <= 0)
            {
                LogHelper.LogInformation(LogMessages.MIML10000);
                return;
            }

            for (int i = 0; i < eventData.Payload.Count; i++)
            {
                _streamWriter.WriteLine(eventData.Payload[i].ToString());
            }
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="TextWriterEventListener"/> class.
        /// </summary>
        public override void Dispose()
        {
            if (_disposeStreamWriter && _streamWriter != null)
            {
                _streamWriter.Flush();
                _streamWriter.Dispose();
            }

            GC.SuppressFinalize(this);
            base.Dispose();
        }
    }
}
