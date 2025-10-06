// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuGet.Common;
using NuGet.LibraryModel;
using NuGet.Versioning;

namespace NuGet.Commands
{
    /// <summary>
    /// Warning and error logging helpers.
    /// </summary>
    public static class DiagnosticUtility
    {
        /// <summary>
        /// Format an id and include the version only if it exists.
        /// Ignore versions for projects.
        /// </summary>
        public static string FormatIdentity(LibraryIdentity identity)
        {
            // Display the version if it exists
            // Ignore versions for projects
            if (identity.Version != null && identity.Type == LibraryType.Package)
            {
                return $"{identity.Name} {identity.Version.ToNormalizedString()}";
            }

            return identity.Name;
        }

        /// <summary>
        /// Format an id and include the range only if it has bounds.
        /// </summary>
        public static string FormatDependency(string id, VersionRange range)
        {
            if (range == null || !(range.HasLowerBound || range.HasUpperBound))
            {
                return id;
            }

            return $"{id} {range.ToNonSnapshotRange().PrettyPrint()}";
        }

        /// <summary>
        /// Format an id and include the lower bound only if it has one.
        /// </summary>
        public static string FormatExpectedIdentity(string id, VersionRange range)
        {
            if (range == null || !range.HasLowerBound || !range.IsMinInclusive)
            {
                return id;
            }

            return $"{id} {range.MinVersion.ToNormalizedString()}";
        }

        /// <summary>
        /// Format a graph name with an optional RID.
        /// </summary>
        public static string FormatGraphName(RestoreTargetGraph graph)
        {
            if (string.IsNullOrEmpty(graph.RuntimeIdentifier))
            {
                return $"({graph.Framework.DotNetFrameworkName})";
            }
            else
            {
                return $"({graph.Framework.DotNetFrameworkName} RuntimeIdentifier: {graph.RuntimeIdentifier})";
            }
        }

        /// <summary>
        /// Format a message as:
        /// 
        /// First line
        ///   - second
        ///   - third
        /// </summary>
        public static string GetMultiLineMessage(IEnumerable<string> lines)
        {
            var sb = new StringBuilder();

            foreach (var line in lines)
            {
                if (sb.Length == 0)
                {
                    sb.Append(line);
                }
                else
                {
                    sb.Append(Environment.NewLine);
                    sb.Append("  - " + line);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Merge messages with the same code and message, combining the target graphs.
        /// </summary>
        public static IEnumerable<RestoreLogMessage> MergeOnTargetGraph(IEnumerable<RestoreLogMessage> messages)
        {
            var output = new List<RestoreLogMessage>();

            foreach (var codeGroup in messages.GroupBy(e => e.Code))
            {
                foreach (var messageGroup in codeGroup.GroupBy(e => e.Message, StringComparer.Ordinal))
                {
                    var group = messageGroup.ToArray();

                    if (group.Length == 1)
                    {
                        output.AddRange(group);
                    }
                    else
                    {
                        var message = new RestoreLogMessage(group[0].Level, group[0].Code, group[0].Message)
                        {
                            Time = group[0].Time,
                            WarningLevel = group[0].WarningLevel,
                            LibraryId = group[0].LibraryId,
                            FilePath = group[0].FilePath,
                            EndColumnNumber = group[0].EndColumnNumber,
                            ProjectPath = group[0].ProjectPath,
                            StartLineNumber = group[0].StartLineNumber,
                            StartColumnNumber = group[0].StartColumnNumber,
                            EndLineNumber = group[0].EndLineNumber,
                            TargetGraphs = group.SelectMany(e => e.TargetGraphs)
                                .OrderBy(e => e, StringComparer.Ordinal)
                                .Distinct()
                                .ToList()
                        };

                        output.Add(message);
                    }
                }
            }

            return output.OrderBy(e => e.Time);
        }
    }
}
