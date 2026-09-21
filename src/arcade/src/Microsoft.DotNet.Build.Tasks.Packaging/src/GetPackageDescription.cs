// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.DotNet.Build.Tasks.Packaging;

public class GetPackageDescription : Task
{
    // avoid parsing the same document multiple times on a single node.
    private static readonly ConcurrentDictionary<string, Dictionary<string, string>> s_descriptionCache = new();

    [Required]
    public ITaskItem DescriptionFile
    {
        get;
        set;
    }

    [Required]
    public string PackageId
    {
        get;
        set;
    }

    [Output]
    public string Description
    {
        get;
        set;
    }

    public override bool Execute()
    {
        if (null == DescriptionFile)
        {
            Log.LogError("DescriptionFile argument must be specified");
            return false;
        }

        if (String.IsNullOrEmpty(PackageId))
        {
            Log.LogError("PackageId argument must be specified");
            return false;
        }

        string descriptionPath = DescriptionFile.GetMetadata("FullPath");

        if (!File.Exists(descriptionPath))
        {
            Log.LogError("DescriptionFile '{0}' does not exist", descriptionPath);
            return false;
        }

        if (!s_descriptionCache.TryGetValue(descriptionPath, out Dictionary<string, string> descriptionTable))
        {
            // no cache, load it now.
            descriptionTable = LoadDescriptions(descriptionPath);

            // Only cache successful loads. LoadDescriptions returns null after logging an
            // IOException or UnauthorizedAccessException, and caching that would memoize a
            // transient failure for every later invocation on this node, including subsequent
            // builds that reuse it.
            if (descriptionTable != null)
            {
                s_descriptionCache.TryAdd(descriptionPath, descriptionTable);
            }
        }

        string description = null;

        if (descriptionTable != null)
        {
            descriptionTable.TryGetValue(PackageId, out description);
        }

        if (String.IsNullOrEmpty(description))
        {
            Log.LogError("Unable to find description for package {0}", PackageId);
        }

        Description = description;

        return !Log.HasLoggedErrors;
    }

    private Dictionary<string, string> LoadDescriptions(string descriptionPath)
    {
        try
        {
            Dictionary<string, string> descriptions = new Dictionary<string, string>();

            var allMetadata = PackageMetadata.ReadFrom(descriptionPath);

            foreach (PackageMetadata metadata in allMetadata)
            {
                descriptions[metadata.Name] = FormatDescription(metadata, descriptionPath);
            }

            return descriptions;
        }
        catch (Exception excep)
        {
            if (excep is IOException || excep is UnauthorizedAccessException)
            {
                Log.LogError("Error loading {0}, {1}", descriptionPath, excep);
                return null;
            }
            else
            {
                throw;
            }
        }
    }

    private string FormatDescription(PackageMetadata metadata, string descriptionPath)
    {
        if (String.IsNullOrEmpty(metadata.Description))
        {
            Log.LogError("Package {0} has no Description, please add it to {1}", metadata.Name, descriptionPath);
        }

        StringBuilder description = new StringBuilder(metadata.Description);

        if (metadata.CommonTypes != null && metadata.CommonTypes.Length > 0)
        {
            description.AppendLine();
            description.AppendLine();
            description.AppendLine("Commonly Used Types:");

            foreach (string type in metadata.CommonTypes)
            {
                description.AppendLine(type);
            }
        }

        return description.ToString();
    }
}
