// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks
{
    /// <summary>
    /// This task adds a source to a well-formed NuGet.Config file with highest
    /// priority, or replaces a source with the same name if present. The task
    /// also by default adds a 'clear' element if none exists, to avoid
    /// unintended leaks from the build environment.
    /// </summary>
    public class AddSourceToNuGetConfig : Task
    {
        [Required]
        public string NuGetConfigFile { get; set; }

        [Required]
        public string SourceName { get; set; }

        [Required]
        public string SourcePath { get; set; }

        public bool SkipEnsureClear { get; set; }

        public override bool Execute()
        {
            XDocument document = XDocument.Load(NuGetConfigFile);

            XName CreateQualifiedName(string plainName)
            {
                return document.Root.GetDefaultNamespace().GetName(plainName);
            }

            XElement packageSourcesElement = document.Root
                .Element(CreateQualifiedName("packageSources"));

            var sourceElementToAdd = new XElement(
                "add",
                new XAttribute("key", SourceName),
                new XAttribute("value", SourcePath));

            XElement existingSourceElement = packageSourcesElement
                .Elements(CreateQualifiedName("add"))
                .FirstOrDefault(e => e.Attribute("key").Value == SourceName);

            XElement lastClearElement = packageSourcesElement
                .Elements(CreateQualifiedName("clear"))
                .LastOrDefault();

            if (existingSourceElement != null)
            {
                existingSourceElement.ReplaceWith(sourceElementToAdd);
            }
            else if (lastClearElement != null)
            {
                lastClearElement.AddAfterSelf(sourceElementToAdd);
            }
            else
            {
                packageSourcesElement.AddFirst(sourceElementToAdd);
            }

            if (lastClearElement == null && !SkipEnsureClear)
            {
                packageSourcesElement.AddFirst(new XElement("clear"));
            }

            using (var fs = new FileStream(NuGetConfigFile, FileMode.Create, FileAccess.ReadWrite))
            {
                document.Save(fs);
            }

            return !Log.HasLoggedErrors;
        }
    }
}
