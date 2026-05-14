// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;
using Microsoft;
using NuGet.Common;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class RemoteErrorFormatter : IMessagePackFormatter<RemoteError?>
    {
        private const string ActivityLogMessagePropertyName = "activitylog";
        private const string LogMessagePropertyName = "logmessage";
        private const string LogMessagesPropertyName = "logmessages";
        private const string ProjectContextLogMessagePropertyName = "projectcontextlogmessage";
        private const string TypeNamePropertyName = "typename";

        internal static readonly IMessagePackFormatter<RemoteError?> Instance = new RemoteErrorFormatter();

        private RemoteErrorFormatter()
        {
        }

        public RemoteError? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            // stack overflow mitigation - see https://github.com/neuecc/MessagePack-CSharp/security/advisories/GHSA-7q36-4xx7-xcxf
            options.Security.DepthStep(ref reader);

            try
            {
                string? activityLogMessage = null;
                ILogMessage? logMessage = null;
                List<ILogMessage>? logMessages = null;
                string? projectContextLogMessage = null;
                string? typeName = null;

                int propertyCount = reader.ReadMapHeader();

                for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
                {
                    switch (reader.ReadString())
                    {
                        case ActivityLogMessagePropertyName:
                            activityLogMessage = reader.ReadString();
                            break;

                        case LogMessagePropertyName:
                            logMessage = ILogMessageFormatter.Instance.Deserialize(ref reader, options);
                            break;

                        case LogMessagesPropertyName:
                            if (!reader.TryReadNil())
                            {
                                logMessages = new List<ILogMessage>();

                                int logMessagesCount = reader.ReadArrayHeader();

                                for (var i = 0; i < logMessagesCount; ++i)
                                {
                                    ILogMessage? lm = ILogMessageFormatter.Instance.Deserialize(ref reader, options);

                                    Assumes.NotNull(lm);

                                    logMessages.Add(lm);
                                }
                            }
                            break;

                        case ProjectContextLogMessagePropertyName:
                            projectContextLogMessage = reader.ReadString();
                            break;

                        case TypeNamePropertyName:
                            typeName = reader.ReadString();
                            break;

                        default:
                            reader.Skip();
                            break;
                    }
                }

                Assumes.NotNullOrEmpty(typeName);
                Assumes.NotNull(logMessage);

                return new RemoteError(typeName, logMessage, logMessages, projectContextLogMessage, activityLogMessage);
            }
            finally
            {
                // stack overflow mitigation - see https://github.com/neuecc/MessagePack-CSharp/security/advisories/GHSA-7q36-4xx7-xcxf
                reader.Depth--;
            }
        }

        public void Serialize(ref MessagePackWriter writer, RemoteError? value, MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            writer.WriteMapHeader(count: 5);
            writer.Write(ActivityLogMessagePropertyName);
            writer.Write(value.ActivityLogMessage);
            writer.Write(LogMessagePropertyName);
            ILogMessageFormatter.Instance.Serialize(ref writer, value.LogMessage, options);

            writer.Write(LogMessagesPropertyName);

            if (value.LogMessages is null)
            {
                writer.WriteNil();
            }
            else
            {
                writer.WriteArrayHeader(value.LogMessages.Count);

                foreach (ILogMessage logMessage in value.LogMessages)
                {
                    ILogMessageFormatter.Instance.Serialize(ref writer, logMessage, options);
                }
            }

            writer.Write(ProjectContextLogMessagePropertyName);
            writer.Write(value.ProjectContextLogMessage);
            writer.Write(TypeNamePropertyName);
            writer.Write(value.TypeName);
        }
    }
}
