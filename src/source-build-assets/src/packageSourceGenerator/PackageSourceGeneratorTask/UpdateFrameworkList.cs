// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.SourceBuild.Tasks;

public class UpdateFrameworkList : Task
{
    [Required]
    public ITaskItem[] Files { get; set; } = null!;

    public override bool Execute()
    {
        foreach (var item in Files)
        {
            var path = item.ItemSpec;

            try
            {
                var doc = XDocument.Load(path);
                var fileElements = doc.Descendants("File")
                    .Where(f => f.Attribute("Type") != null &&
                                (string?)f.Attribute("Type") == "Analyzer")
                    .ToList();

                foreach (var file in fileElements)
                {
                    file.Remove();
                }

                var settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Indent = true
                };

                using (var writer = XmlWriter.Create(path, settings))
                {
                    doc.Save(writer);
                }

                Log.LogMessage(MessageImportance.Low, $"Updated: {path}");
            }
            catch (Exception ex)
            {
                Log.LogError($"Error processing {path}: {ex.Message}");
                return false;
            }
        }

        return !Log.HasLoggedErrors;
    }
}
