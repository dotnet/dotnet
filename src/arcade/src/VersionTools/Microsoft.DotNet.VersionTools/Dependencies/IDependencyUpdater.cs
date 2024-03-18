// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Microsoft.DotNet.VersionTools.Dependencies
{
    /// <summary>
    /// A tool that uses dependency infos to perform an update.
    /// </summary>
    public interface IDependencyUpdater
    {
        /// <summary>
        /// Returns update tasks based on the dependency infos passed.
        /// </summary>
        IEnumerable<DependencyUpdateTask> GetUpdateTasks(IEnumerable<IDependencyInfo> dependencyInfos);
    }
}
