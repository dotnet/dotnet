// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Task = Microsoft.Build.Utilities.Task;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Represents a logging queue for messages that eventually sent to a <see cref="TaskLoggingHelper" />.
    /// </summary>
    internal class TaskLoggingQueue : LoggingQueue<string>
    {
        /// <summary>
        /// Stores the list of files to embed in the MSBuild binary log.
        /// </summary>
        private readonly List<string> _filesToEmbedInBinlog = new List<string>();

        /// <summary>
        /// The <see cref="TaskLoggingHelper" /> to log messages to.
        /// </summary>
        private readonly TaskLoggingHelper _log;

        /// <summary>
        /// A <see cref="CustomCreationConverter{T}" /> to use when deserializing JSON strings as <see cref="ConsoleOutLogItem" /> objects.
        /// </summary>
        private readonly ConsoleOutLogItemConverter _converter = new ConsoleOutLogItemConverter();

        /// <summary>
        /// Initializes a new instance of the TaskLoggingHelperQueue class.
        /// </summary>
        /// <param name="taskLoggingHelper">The <see cref="Task" /> to create a logging queue for.</param>
        public TaskLoggingQueue(TaskLoggingHelper taskLoggingHelper)
        {
            _log = taskLoggingHelper ?? throw new ArgumentNullException(nameof(taskLoggingHelper));
        }

        public IReadOnlyCollection<string> FilesToEmbedInBinlog => _filesToEmbedInBinlog;

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
                ConsoleOutLogItem consoleOutLogItem;

                try
                {
                    consoleOutLogItem = JsonConvert.DeserializeObject<ConsoleOutLogItem>(message, _converter);
                }
                catch (ArgumentOutOfRangeException)
                {
                    // Should only be thrown if the MessageType is unrecognized
                    throw;
                }
                catch (Exception)
                {
                    // Log the raw message if it couldn't be deserialized
                    _log.LogMessageFromText(message, MessageImportance.Low);

                    return;
                }

                if (consoleOutLogItem is ConsoleOutLogEmbedInBinlog consoleOutEmbedInBinlog)
                {
                    _filesToEmbedInBinlog.Add(consoleOutEmbedInBinlog.Path);

                    return;
                }

                if (consoleOutLogItem is ConsoleOutLogMessage consoleOutLogMessage)
                {
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
                            _log.LogMessageFromText(consoleOutLogMessage.Message, (MessageImportance)consoleOutLogMessage.Importance);
                            return;
                    }
                }
            }
        }

        /// <summary>
        /// Represents a <see cref="CustomCreationConverter{T}" /> for converting JSON strings to a <see cref="ConsoleOutLogMessage" /> or a <see cref="ConsoleOutLogEmbedInBinlog" /> object.
        /// </summary>
        private class ConsoleOutLogItemConverter : CustomCreationConverter<ConsoleOutLogItem>
        {
            private ConsoleOutLogMessageType _consoleOutLogMessageType;

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JToken token = JObject.ReadFrom(reader);

                _consoleOutLogMessageType = token[nameof(ConsoleOutLogItem.MessageType)].ToObject<ConsoleOutLogMessageType>();

                return base.ReadJson(token.CreateReader(), objectType, existingValue, serializer);
            }

            public override ConsoleOutLogItem Create(Type objectType)
            {
                switch (_consoleOutLogMessageType)
                {
                    case ConsoleOutLogMessageType.Message:
                    case ConsoleOutLogMessageType.Warning:
                    case ConsoleOutLogMessageType.Error:
                        return new ConsoleOutLogMessage
                        {
                            MessageType = _consoleOutLogMessageType
                        };

                    case ConsoleOutLogMessageType.EmbedInBinlog:
                        return new ConsoleOutLogEmbedInBinlog();

                    default:
                        throw new ArgumentOutOfRangeException(paramName: nameof(ConsoleOutLogItem.MessageType), $"Invalid message type '{_consoleOutLogMessageType}'");
                }
            }
        }
    }
}
