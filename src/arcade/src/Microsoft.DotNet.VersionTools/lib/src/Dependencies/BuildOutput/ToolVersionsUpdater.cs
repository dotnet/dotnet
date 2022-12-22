// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.VersionTools.Dependencies.BuildOutput
{
    public class ToolVersionsUpdater : IDependencyUpdater
    {
        public string Path { get; set; }

        public IEnumerable<DependencyUpdateTask> GetUpdateTasks(
            IEnumerable<IDependencyInfo> dependencyInfos)
        {
            BuildDependencyInfo[] buildDependencyInfos = dependencyInfos
                .OfType<BuildDependencyInfo>()
                .ToArray();

            var lineResults = new List<ToolUpdateLineResult>();

            Action updateTask = FileUtils.GetUpdateFileContentsTask(
                Path,
                content =>
                {
                    var reader = new StringReader(content);
                    var writer = new StringWriter();

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var result = new ToolUpdateLineResult(line, buildDependencyInfos);
                        writer.WriteLine(result.Content);
                        if (result.UsedInfo != null)
                        {
                            lineResults.Add(result);
                        }
                    }

                    return writer.ToString();
                });

            if (updateTask != null)
            {
                yield return new DependencyUpdateTask(
                    updateTask,
                    lineResults.Select(c => c.UsedInfo),
                    lineResults.Select(c => $"In '{Path}', '{c.ToolName}' '{c.OriginalVersion}' must be '{c.NewVersion}'."));
            }
        }

        private class ToolUpdateLineResult
        {
            public string Content { get; }

            public BuildDependencyInfo UsedInfo { get; }

            public string ToolName { get; }

            public string OriginalVersion { get; }

            public string NewVersion { get; }

            public ToolUpdateLineResult(string line, IEnumerable<BuildDependencyInfo> buildInfos)
            {
                Content = line;

                int separatorIndex = line.IndexOf('=');
                if (separatorIndex == -1)
                {
                    // Ignore lines without a 'name=version' string.
                    return;
                }

                string name = line.Substring(0, separatorIndex).Trim();
                string version = line.Substring(separatorIndex + 1);

                OriginalVersion = version;
                ToolName = name;

                foreach (var info in buildInfos)
                {
                    if (info.RawPackages.TryGetValue(name, out version))
                    {
                        Content = $"{name}={version}";
                        NewVersion = version;
                        UsedInfo = info;
                        break;
                    }
                }
            }
        }
    }
}
