// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Services.Common;
using NuGet.Frameworks;
using NuGet.ProjectManagement;

namespace NuGet.SolutionRestoreManager.Test;

internal class TestProjectRestoreInfoBuilder
{
    List<IVsTargetFrameworkInfo4> _targetFrameworks = new();
    List<VsReferenceItem2>? _toolReferences = null;

    public IVsProjectRestoreInfo3 Build()
    {
        if (_targetFrameworks.Count == 0)
        {
            throw new InvalidOperationException("At least one target framework must be added before building the project restore info.");
        }

        var originalTargetFrameworks = string.Join(";", _targetFrameworks.Select(tf => tf.Properties[ProjectBuildProperties.TargetFramework]));

        var pri = new VsProjectRestoreInfo3
        {
            MSBuildProjectExtensionsPath = @"y:\src\some\project\obj",
            TargetFrameworks = _targetFrameworks,
            ToolReferences = _toolReferences,
            OriginalTargetFrameworks = originalTargetFrameworks
        };
        return pri;
    }

    public TestProjectRestoreInfoBuilder WithTargetFrameworkInfo(
        string targetFramework,
        Action<TargetFrameworkBuilder>? builder = null)
    {
        TargetFrameworkBuilder tfBuilder = new TargetFrameworkBuilder()
            .WithProperty(ProjectBuildProperties.MSBuildProjectExtensionsPath, @"y:\src\some\project\obj")
            .WithProperty("MSBuildProjectFullPath", @"y:\src\some\project\project.csproj");

        var tf = NuGetFramework.Parse(targetFramework);
        var tfi = tf.Framework;
        var version = tf.Version.ToString();

        tfBuilder
            .WithProperty(ProjectBuildProperties.TargetFramework, targetFramework!)
            .WithProperty(ProjectBuildProperties.TargetFrameworkIdentifier, tfi)
            .WithProperty(ProjectBuildProperties.TargetFrameworkVersion, version)
            .WithProperty(ProjectBuildProperties.TargetFrameworkMoniker, tf.DotNetFrameworkName);

        if (!string.IsNullOrEmpty(tf.Platform))
        {
            tfBuilder
                .WithProperty(ProjectBuildProperties.TargetPlatformIdentifier, tf.Platform)
                .WithProperty(ProjectBuildProperties.TargetPlatformVersion, tf.PlatformVersion?.ToString() ?? string.Empty)
                .WithProperty(ProjectBuildProperties.TargetPlatformMoniker, tf.DotNetPlatformName);
        }

        if (builder != null)
        {
            builder(tfBuilder);
        }

        var targetFrameworkInfo = tfBuilder.Build();
        _targetFrameworks.Add(targetFrameworkInfo);

        return this;
    }

    public TestProjectRestoreInfoBuilder WithTool(string toolName, string toolVersion)
    {
        if (_toolReferences is null)
        {
            _toolReferences = new List<VsReferenceItem2>();
        }

        var toolReference = new VsReferenceItem2
        {
            Name = toolName,
            Metadata = new Dictionary<string, string>
            {
            { "Version", toolVersion },
            }
        };
        _toolReferences.Add(toolReference);

        return this;
    }

    private record VsProjectRestoreInfo3 : IVsProjectRestoreInfo3
    {
        public required string MSBuildProjectExtensionsPath { get; init; }
        public required IReadOnlyList<IVsTargetFrameworkInfo4> TargetFrameworks { get; init; }
        public required IReadOnlyList<IVsReferenceItem2>? ToolReferences { get; init; }
        public required string? OriginalTargetFrameworks { get; init; }
    }

    private record VsReferenceItem2 : IVsReferenceItem2
    {
        public required string Name { get; init; }
        public required IReadOnlyDictionary<string, string> Metadata { get; init; }
    }

    private record VsTargetFrameworkInfo4 : IVsTargetFrameworkInfo4
    {
        public required IReadOnlyDictionary<string, string> Properties { get; init; }
        public required IReadOnlyDictionary<string, IReadOnlyList<IVsReferenceItem2>> Items { get; init; }
    }

    internal class TargetFrameworkBuilder
    {
        private Dictionary<string, string> Properties { get; } = new(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, IReadOnlyList<IVsReferenceItem2>> Items { get; } = new(StringComparer.OrdinalIgnoreCase);

        public TargetFrameworkBuilder WithProperty(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Properties.Remove(key);
            }
            else
            {
                Properties[key] = value;
            }
            return this;
        }

        public TargetFrameworkBuilder WithItem(string itemType, string itemName, KeyValuePair<string, string>[] metadata)
        {
            List<IVsReferenceItem2> items;
            if (Items.TryGetValue(itemType, out var existingItems))
            {
                items = (List<IVsReferenceItem2>)existingItems;
            }
            else
            {
                items = new List<IVsReferenceItem2>();
                Items[itemType] = items;
            }

            var itemMetadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            itemMetadata.AddRange(metadata);

            var newItem = new VsReferenceItem2
            {
                Name = itemName,
                Metadata = itemMetadata
            };
            items.Add(newItem);

            return this;
        }

        internal IVsTargetFrameworkInfo4 Build()
        {
            if (Properties.Count == 0)
            {
                throw new InvalidOperationException("At least one property must be added before building the target framework info.");
            }

            return new VsTargetFrameworkInfo4
            {
                Items = Items,
                Properties = Properties
            };
        }
    }
}
