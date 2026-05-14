// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Build.Execution;

namespace NuGet.Build.Tasks.Console
{
    /// <summary>
    /// Represents an <see cref="MSBuildItemBase" /> implementation which wraps a <see cref="ProjectItemInstance" /> object.
    /// </summary>
    internal sealed class MSBuildProjectItemInstance : MSBuildItemBase
    {
        /// <summary>
        /// Gets the <see cref="ProjectItemInstance" /> being wrapped.
        /// </summary>
        private readonly ProjectItemInstance _projectItemInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSBuildProjectItemInstance" /> class.
        /// </summary>
        /// <param name="projectItemInstance">The <see cref="ProjectItemInstance" /> to wrap.</param>
        public MSBuildProjectItemInstance(ProjectItemInstance projectItemInstance)
        {
            _projectItemInstance = projectItemInstance ?? throw new ArgumentNullException(nameof(projectItemInstance));
        }

        public override string Identity => _projectItemInstance.EvaluatedInclude;

        /// <inheritdoc cref="MSBuildItemBase.GetPropertyValue(string)" />
        protected override string GetPropertyValue(string name) => _projectItemInstance.GetMetadataValue(name);
    }
}
