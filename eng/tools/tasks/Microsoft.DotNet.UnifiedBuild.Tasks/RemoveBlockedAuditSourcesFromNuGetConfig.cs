// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.UnifiedBuild.Tasks
{
    /// <summary>
    /// Removes blocked entries from the auditSources section of a NuGet.config file.
    /// Audit sources can reach out to the internet at restore time which may not be available in CI builds.
    /// </summary>
    [MSBuildMultiThreadableTask]
    public class RemoveBlockedAuditSourcesFromNuGetConfig : Task, IMultiThreadableTask
    {
        /// <summary>Injected by MSBuild so paths resolve against the project directory in multithreaded builds.</summary>
        public TaskEnvironment TaskEnvironment { get; set; } = TaskEnvironment.Fallback;

        private static readonly string[] BlockedAuditSources = [ "nuget.org" ];

        [Required]
        public required string NuGetConfigFile { get; set; }

        public override bool Execute()
        {
            string xml = File.ReadAllText(TaskEnvironment.GetAbsolutePath(NuGetConfigFile));
            string newLineChars = FileUtilities.DetectNewLineChars(xml);
            XDocument d = XDocument.Parse(xml);

            XElement? auditSourcesElement = d.Root?.Descendants().FirstOrDefault(e => e.Name == "auditSources");
            if (auditSourcesElement == null)
            {
                return true;
            }

            foreach (string url in BlockedAuditSources)
            {
                auditSourcesElement.Descendants("add")
                    .FirstOrDefault(e => e.Attribute("value")?.Value?.Contains(url, StringComparison.OrdinalIgnoreCase) == true)
                    ?.Remove();
            }

            using (var w = XmlWriter.Create(TaskEnvironment.GetAbsolutePath(NuGetConfigFile), new XmlWriterSettings { NewLineChars = newLineChars, Indent = true }))
            {
                d.Save(w);
            }

            return true;
        }
    }
}
