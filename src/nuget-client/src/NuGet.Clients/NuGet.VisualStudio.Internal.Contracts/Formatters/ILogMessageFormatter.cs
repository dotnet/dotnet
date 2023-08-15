// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;
using Microsoft;
using NuGet.Common;
using NuGet.Packaging.Signing;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class ILogMessageFormatter : IMessagePackFormatter<ILogMessage?>
    {
        private const string CodePropertyName = "code";
        private const string EndColumnNumberPropertyName = "endcolumnnumber";
        private const string EndLineNumberPropertyName = "endlinenumber";
        private const string FilePathPropertyName = "filepath";
        private const string LevelPropertyName = "level";
        private const string LibraryIdPropertyName = "libraryid";
        private const string MessagePropertyName = "message";
        private const string ProjectPathPropertyName = "projectpath";
        private const string ShouldDisplayPropertyName = "shoulddisplay";
        private const string StartColumnNumberPropertyName = "startcolumnnumber";
        private const string StartLineNumberPropertyName = "startlinenumber";
        private const string TargetGraphsPropertyName = "targetgraphs";
        private const string TimePropertyName = "time";
        private const string TypeNamePropertyName = "typename";
        private const string WarningLevelPropertyName = "warninglevel";

        private static readonly string LogMessageTypeName = typeof(LogMessage).Name;
        private static readonly string PackagingLogMessageTypeName = typeof(PackagingLogMessage).Name;
        private static readonly string RestoreLogMessageTypeName = typeof(RestoreLogMessage).Name;
        private static readonly string SignatureLogTypeName = typeof(SignatureLog).Name;

        internal static readonly IMessagePackFormatter<ILogMessage?> Instance = new ILogMessageFormatter();

        private ILogMessageFormatter()
        {
        }

        public ILogMessage? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            // stack overflow mitigation - see https://github.com/neuecc/MessagePack-CSharp/security/advisories/GHSA-7q36-4xx7-xcxf
            options.Security.DepthStep(ref reader);

            try
            {
                NuGetLogCode? code = null;
                int? endColumnNumber = null;
                int? endLineNumber = null;
                string? filePath = null;
                string? libraryId = null;
                LogLevel? logLevel = null;
                string? message = null;
                string? projectPath = null;
                bool? shouldDisplay = null;
                int? startColumnNumber = null;
                int? startLineNumber = null;
                IReadOnlyList<string> targetGraphs = Array.Empty<string>();
                DateTimeOffset? time = null;
                string? typeName = null;
                WarningLevel? warningLevel = null;

                int propertyCount = reader.ReadMapHeader();

                for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
                {
                    switch (reader.ReadString())
                    {
                        case CodePropertyName:
                            code = options.Resolver.GetFormatter<NuGetLogCode>().Deserialize(ref reader, options);
                            break;

                        case EndColumnNumberPropertyName:
                            endColumnNumber = reader.ReadInt32();
                            break;

                        case EndLineNumberPropertyName:
                            endLineNumber = reader.ReadInt32();
                            break;

                        case FilePathPropertyName:
                            filePath = reader.ReadString();
                            break;

                        case LevelPropertyName:
                            logLevel = options.Resolver.GetFormatter<LogLevel>().Deserialize(ref reader, options);
                            break;

                        case LibraryIdPropertyName:
                            libraryId = reader.ReadString();
                            break;

                        case MessagePropertyName:
                            message = reader.ReadString();
                            break;

                        case ProjectPathPropertyName:
                            projectPath = reader.ReadString();
                            break;

                        case ShouldDisplayPropertyName:
                            shouldDisplay = reader.ReadBoolean();
                            break;

                        case StartColumnNumberPropertyName:
                            startColumnNumber = reader.ReadInt32();
                            break;

                        case StartLineNumberPropertyName:
                            startLineNumber = reader.ReadInt32();
                            break;

                        case TargetGraphsPropertyName:
                            if (!reader.TryReadNil())
                            {
                                var list = new List<string>();

                                int targetGraphsCount = reader.ReadArrayHeader();

                                for (var i = 0; i < targetGraphsCount; ++i)
                                {
                                    string targetGraph = reader.ReadString();

                                    list.Add(targetGraph);
                                }

                                targetGraphs = list;
                            }
                            break;

                        case TimePropertyName:
                            time = options.Resolver.GetFormatter<DateTimeOffset>().Deserialize(ref reader, options);
                            break;

                        case TypeNamePropertyName:
                            typeName = reader.ReadString();
                            break;

                        case WarningLevelPropertyName:
                            warningLevel = options.Resolver.GetFormatter<WarningLevel>().Deserialize(ref reader, options);
                            break;

                        default:
                            reader.Skip();
                            break;
                    }
                }

                Assumes.NotNullOrEmpty(typeName);
                Assumes.True(code.HasValue);
                Assumes.True(logLevel.HasValue);
                Assumes.NotNull(message);
                Assumes.True(time.HasValue);
                Assumes.True(warningLevel.HasValue);

                ILogMessage? logMessage = null;

                if (typeName == LogMessageTypeName)
                {
                    logMessage = new LogMessage(logLevel.Value, message)
                    {
                        Code = code.Value,
                        ProjectPath = projectPath,
                        Time = time.Value,
                        WarningLevel = warningLevel.Value
                    };
                }
                else if (typeName == PackagingLogMessageTypeName)
                {
                    Assumes.True(endColumnNumber.HasValue);
                    Assumes.True(endLineNumber.HasValue);
                    Assumes.True(startColumnNumber.HasValue);
                    Assumes.True(startLineNumber.HasValue);

                    PackagingLogMessage packagingLogMessage = PackagingLogMessage.CreateError(message, code.Value);

                    packagingLogMessage.Code = code.Value;
                    packagingLogMessage.EndColumnNumber = endColumnNumber.Value;
                    packagingLogMessage.EndLineNumber = endLineNumber.Value;
                    packagingLogMessage.FilePath = filePath;
                    packagingLogMessage.Level = logLevel.Value;
                    packagingLogMessage.ProjectPath = projectPath;
                    packagingLogMessage.StartColumnNumber = startColumnNumber.Value;
                    packagingLogMessage.StartLineNumber = startLineNumber.Value;
                    packagingLogMessage.Time = time.Value;
                    packagingLogMessage.WarningLevel = warningLevel.Value;

                    logMessage = packagingLogMessage;
                }
                else if (typeName == RestoreLogMessageTypeName)
                {
                    Assumes.True(endColumnNumber.HasValue);
                    Assumes.True(endLineNumber.HasValue);
                    Assumes.True(shouldDisplay.HasValue);
                    Assumes.True(startColumnNumber.HasValue);
                    Assumes.True(startLineNumber.HasValue);

                    logMessage = new RestoreLogMessage(logLevel.Value, message)
                    {
                        Code = code.Value,
                        EndColumnNumber = endColumnNumber.Value,
                        EndLineNumber = endLineNumber.Value,
                        FilePath = filePath,
                        Level = logLevel.Value,
                        LibraryId = libraryId,
                        Message = message,
                        ProjectPath = projectPath,
                        ShouldDisplay = shouldDisplay.Value,
                        StartColumnNumber = startColumnNumber.Value,
                        StartLineNumber = startLineNumber.Value,
                        TargetGraphs = targetGraphs,
                        Time = time.Value,
                        WarningLevel = warningLevel.Value
                    };
                }
                else if (typeName == SignatureLogTypeName)
                {
                    SignatureLog signatureLog = SignatureLog.Error(code.Value, message);

                    signatureLog.Code = code.Value;
                    signatureLog.Level = logLevel.Value;
                    signatureLog.LibraryId = libraryId;
                    signatureLog.ProjectPath = projectPath;
                    signatureLog.Time = time.Value;
                    signatureLog.WarningLevel = warningLevel.Value;

                    logMessage = signatureLog;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                return logMessage;
            }
            finally
            {
                // stack overflow mitigation - see https://github.com/neuecc/MessagePack-CSharp/security/advisories/GHSA-7q36-4xx7-xcxf
                reader.Depth--;
            }
        }

        public void Serialize(ref MessagePackWriter writer, ILogMessage? value, MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            if (value is LogMessage logMessage)
            {
                Serialize(ref writer, logMessage, options);
            }
            else if (value is RestoreLogMessage restoreLogMessage)
            {
                Serialize(ref writer, restoreLogMessage, options);
            }
            else if (value is SignatureLog signatureLogMessage)
            {
                Serialize(ref writer, signatureLogMessage, options);
            }
            else if (value is PackagingLogMessage packagingLogMessage)
            {
                Serialize(ref writer, packagingLogMessage, options);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private void SerializeCommonProperties(ref MessagePackWriter writer, ILogMessage value, MessagePackSerializerOptions options)
        {
            writer.Write(TypeNamePropertyName);
            writer.Write(value.GetType().Name);
            writer.Write(CodePropertyName);
            options.Resolver.GetFormatter<NuGetLogCode>().Serialize(ref writer, value.Code, options);
            writer.Write(LevelPropertyName);
            options.Resolver.GetFormatter<LogLevel>().Serialize(ref writer, value.Level, options);
            writer.Write(MessagePropertyName);
            writer.Write(value.Message);
            writer.Write(ProjectPathPropertyName);
            writer.Write(value.ProjectPath);
            writer.Write(TimePropertyName);
            options.Resolver.GetFormatter<DateTimeOffset>().Serialize(ref writer, value.Time, options);
            writer.Write(WarningLevelPropertyName);
            options.Resolver.GetFormatter<WarningLevel>().Serialize(ref writer, value.WarningLevel, options);
        }

        private void Serialize(ref MessagePackWriter writer, LogMessage logMessage, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 7);

            SerializeCommonProperties(ref writer, logMessage, options);
        }

        private void Serialize(ref MessagePackWriter writer, PackagingLogMessage logMessage, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 12);

            SerializeCommonProperties(ref writer, logMessage, options);

            writer.Write(EndColumnNumberPropertyName);
            writer.Write(logMessage.EndColumnNumber);
            writer.Write(EndLineNumberPropertyName);
            writer.Write(logMessage.EndLineNumber);
            writer.Write(FilePathPropertyName);
            writer.Write(logMessage.FilePath);
            writer.Write(StartColumnNumberPropertyName);
            writer.Write(logMessage.StartColumnNumber);
            writer.Write(StartLineNumberPropertyName);
            writer.Write(logMessage.StartLineNumber);
        }

        private void Serialize(ref MessagePackWriter writer, RestoreLogMessage logMessage, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 15);

            SerializeCommonProperties(ref writer, logMessage, options);

            writer.Write(EndColumnNumberPropertyName);
            writer.Write(logMessage.EndColumnNumber);
            writer.Write(EndLineNumberPropertyName);
            writer.Write(logMessage.EndLineNumber);
            writer.Write(FilePathPropertyName);
            writer.Write(logMessage.FilePath);
            writer.Write(LibraryIdPropertyName);
            writer.Write(logMessage.LibraryId);
            writer.Write(ShouldDisplayPropertyName);
            writer.Write(logMessage.ShouldDisplay);
            writer.Write(StartColumnNumberPropertyName);
            writer.Write(logMessage.StartColumnNumber);
            writer.Write(StartLineNumberPropertyName);
            writer.Write(logMessage.StartLineNumber);
            writer.Write(TargetGraphsPropertyName);

            if (logMessage.TargetGraphs is null)
            {
                writer.WriteNil();
            }
            else
            {
                writer.WriteArrayHeader(logMessage.TargetGraphs.Count);

                foreach (string targetGraph in logMessage.TargetGraphs)
                {
                    writer.Write(targetGraph);
                }
            }
        }

        private void Serialize(ref MessagePackWriter writer, SignatureLog logMessage, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 8);

            SerializeCommonProperties(ref writer, logMessage, options);

            writer.Write(LibraryIdPropertyName);
            writer.Write(logMessage.LibraryId);
        }
    }
}
