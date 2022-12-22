// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Execution;
using NuGet.Commands;

namespace NuGet.Build.Tasks.Console
{
    /// <summary>
    /// Represents an <see cref="MSBuildItemBase" /> implementation which wraps a <see cref="ProjectInstance" /> object.
    /// </summary>
    internal sealed class MSBuildProjectInstance : MSBuildItemBase, IMSBuildProject
    {
        /// <summary>
        /// Gets the <see cref="ProjectInstance" /> being wrapped.
        /// </summary>
        private readonly ProjectInstance _projectInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSBuildProjectInstance" /> class.
        /// </summary>
        /// <param name="projectInstance">The <see cref="ProjectInstance" /> to wrap.</param>
        public MSBuildProjectInstance(ProjectInstance projectInstance)
        {
            _projectInstance = projectInstance ?? throw new ArgumentNullException(nameof(projectInstance));
        }

        /// <summary>
        /// Gets the full path to the directory that contains the project.
        /// </summary>
        public string Directory => _projectInstance.Directory;

        /// <summary>
        /// Gets the full path to the project.
        /// </summary>
        public string FullPath => _projectInstance.FullPath;

        public static implicit operator MSBuildProjectInstance(ProjectInstance projectInstance)
        {
            return new MSBuildProjectInstance(projectInstance);
        }

        public IEnumerable<IMSBuildItem> GetItems(string name)
        {
            return _projectInstance.GetItems(name).Select(i => new MSBuildProjectItemInstance(i));
        }

        /// <inheritdoc cref="MSBuildItemBase.GetPropertyValue(string)" />
        protected override string GetPropertyValue(string name) => _projectInstance.GetPropertyValue(name);

        public string GetGlobalProperty(string property)
        {
            string value = GetPropertyValue(property).Trim();

            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return value;
        }
    }
}
