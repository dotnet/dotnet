// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using NuGet.Commands.Restore;

namespace NuGet.Build.Tasks.Console
{
    internal class RestoreProjectAdapter : IProject
    {
        public RestoreProjectAdapter(string fullPath)
        {
            FullPath = fullPath ?? throw new ArgumentNullException(nameof(fullPath));
            Directory = Path.GetDirectoryName(fullPath) ?? throw new ArgumentNullException(nameof(fullPath));

            _targetFrameworks = new(StringComparer.OrdinalIgnoreCase);
        }

        public string FullPath { get; }

        public string Directory { get; }

        public ITargetFramework OuterBuild { get; private set; }

        public IReadOnlyDictionary<string, ITargetFramework> TargetFrameworks => _targetFrameworks;

        private ConcurrentDictionary<string, ITargetFramework> _targetFrameworks;

        internal void AddTargetFramework(string targetFramework, ITargetFramework targetFrameworkInstance)
        {
            if (string.IsNullOrWhiteSpace(targetFramework))
            {
                OuterBuild = targetFrameworkInstance;
            }
            else
            {
                _targetFrameworks.TryAdd(targetFramework, targetFrameworkInstance);
            }
        }

        internal void Prepare()
        {
            if (TargetFrameworks.Count == 0)
            {
                var targetFramework = OuterBuild.GetProperty("TargetFramework") ?? string.Empty;
                _targetFrameworks.TryAdd(targetFramework, OuterBuild);
            }
        }
    }
}
