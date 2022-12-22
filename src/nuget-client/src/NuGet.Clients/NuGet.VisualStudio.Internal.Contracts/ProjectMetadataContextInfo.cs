// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using NuGet.ProjectManagement;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public sealed class ProjectMetadataContextInfo : IProjectMetadataContextInfo
    {
        public string? FullPath { get; }
        public string? Name { get; }
        public string? ProjectId { get; }
        public IReadOnlyCollection<NuGetFramework>? SupportedFrameworks { get; }
        public NuGetFramework? TargetFramework { get; }
        public string? UniqueName { get; }

        internal ProjectMetadataContextInfo(
            string? fullPath,
            string? name,
            string? projectId,
            IReadOnlyCollection<NuGetFramework>? supportedFrameworks,
            NuGetFramework? targetFramework,
            string? uniqueName)
        {
            FullPath = fullPath;
            Name = name;
            ProjectId = projectId;
            SupportedFrameworks = supportedFrameworks;
            TargetFramework = targetFramework;
            UniqueName = uniqueName;
        }

        public static ProjectMetadataContextInfo Create(IReadOnlyDictionary<string, object?> projectMetadata)
        {
            if (projectMetadata is null)
            {
                throw new ArgumentNullException(nameof(projectMetadata));
            }

            string? fullPath = null;
            string? name = null;
            string? projectId = null;
            IReadOnlyCollection<NuGetFramework>? supportedFrameworks = null;
            NuGetFramework? targetFramework = null;
            string? uniqueName = null;
            object? value;

            if (projectMetadata.TryGetValue(NuGetProjectMetadataKeys.FullPath, out value))
            {
                fullPath = value as string;
            }

            if (projectMetadata.TryGetValue(NuGetProjectMetadataKeys.Name, out value))
            {
                name = value as string;
            }

            if (projectMetadata.TryGetValue(NuGetProjectMetadataKeys.ProjectId, out value))
            {
                projectId = value as string;
            }

            if (projectMetadata.TryGetValue(NuGetProjectMetadataKeys.SupportedFrameworks, out value))
            {
                if (value is IEnumerable<NuGetFramework> frameworks)
                {
                    supportedFrameworks = frameworks.ToArray();
                }
            }

            if (projectMetadata.TryGetValue(NuGetProjectMetadataKeys.TargetFramework, out value))
            {
                targetFramework = ToNuGetFramework(value);
            }

            if (projectMetadata.TryGetValue(NuGetProjectMetadataKeys.UniqueName, out value))
            {
                uniqueName = value as string;
            }

            return new ProjectMetadataContextInfo(
                fullPath,
                name,
                projectId,
                supportedFrameworks,
                targetFramework,
                uniqueName);
        }

        private static NuGetFramework? ToNuGetFramework(object? value)
        {
            // See https://github.com/NuGet/Home/issues/4491
            if (value is string stringValue)
            {
                return NuGetFramework.ParseFrameworkName(stringValue, DefaultFrameworkNameProvider.Instance);
            }

            if (value is NuGetFramework frameworkValue)
            {
                return frameworkValue;
            }

            return null;
        }
    }
}
