// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using Task = Microsoft.Build.Utilities.Task;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Represents a logging queue for messages that eventually sent to a <see cref="TaskLoggingHelper" />.
    /// </summary>
    internal class TaskLoggingQueue : LoggingQueue<string>
    {
        /// <summary>
        /// The <see cref="TaskLoggingHelper" /> to log messages to.
        /// </summary>
        private readonly TaskLoggingHelper _log;

        /// <summary>
        /// Initializes a new instance of the TaskLoggingHelperQueue class.
        /// </summary>
        /// <param name="taskLoggingHelper">The <see cref="Task" /> to create a logging queue for.</param>
        public TaskLoggingQueue(TaskLoggingHelper taskLoggingHelper)
        {
            _log = taskLoggingHelper ?? throw new ArgumentNullException(nameof(taskLoggingHelper));
        }

        /// <summary>
        /// Processes the specified logging message and logs in with a <see cref="TaskLoggingHelper" />.
        /// </summary>
        /// <param name="message">The JSON message to log.</param>
        protected override void Process(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                // Ignore messages that are null or empty lines.  Actual empty lines will still come in as JSON objects.
                return;
            }

            // Check if the message is JSON before attempting to deserialize it
            if (message.Length >= 2 && message[0] == '{' && message[message.Length - 1] == '}')
            {
                ConsoleOutLogMessage consoleOutLogMessage;

                try
                {
                    consoleOutLogMessage = JsonConvert.DeserializeObject<ConsoleOutLogMessage>(message);
                }
                catch (Exception)
                {
                    // Log the raw message if it couldn't be deserialized
                    _log.LogMessageFromText(message, MessageImportance.Low);

                    return;
                }

                // Convert the ConsoleOutLogMessage object to the corresponding MSBuild event object and log it
                switch (consoleOutLogMessage.MessageType)
                {
                    case ConsoleOutLogMessageType.Error:
                        _log.LogError(
                            subcategory: consoleOutLogMessage.Subcategory,
                            errorCode: consoleOutLogMessage.Code,
                            helpKeyword: consoleOutLogMessage.HelpKeyword,
                            file: consoleOutLogMessage.File,
                            lineNumber: consoleOutLogMessage.LineNumber,
                            columnNumber: consoleOutLogMessage.ColumnNumber,
                            endLineNumber: consoleOutLogMessage.EndLineNumber,
                            endColumnNumber: consoleOutLogMessage.EndColumnNumber,
                            message: consoleOutLogMessage.Message);
                        return;

                    case ConsoleOutLogMessageType.Warning:
                        _log.LogWarning(
                            subcategory: consoleOutLogMessage.Subcategory,
                            warningCode: consoleOutLogMessage.Code,
                            helpKeyword: consoleOutLogMessage.HelpKeyword,
                            file: consoleOutLogMessage.File,
                            lineNumber: consoleOutLogMessage.LineNumber,
                            columnNumber: consoleOutLogMessage.ColumnNumber,
                            endLineNumber: consoleOutLogMessage.EndLineNumber,
                            endColumnNumber: consoleOutLogMessage.EndColumnNumber,
                            message: consoleOutLogMessage.Message);
                        return;

                    case ConsoleOutLogMessageType.Message:
                        _log.LogMessageFromText(consoleOutLogMessage.Message, consoleOutLogMessage.Importance);
                        return;

                    default:
                        throw new ArgumentOutOfRangeException(
                            paramName: nameof(message),
                            message: nameof(consoleOutLogMessage.MessageType));
                }
            }
        }
    }
}
