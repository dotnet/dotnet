// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Automation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.DotNet.VersionTools.Dependencies
{
    /// <summary>
    /// A task that can be performed to update dependency versions to values in BuildInfos. Has
    /// properties that describe the task to be performed.
    /// </summary>
    public class DependencyUpdateTask : Task<DependencyUpdateResults>
    {
        /// <summary>
        /// The dependency infos that were used to create this update task.
        /// </summary>
        public IEnumerable<IDependencyInfo> UsedInfos { get; }

        public IEnumerable<string> ReadableDescriptionLines { get; }

        public DependencyUpdateTask(
            Action updateAction,
            IEnumerable<IDependencyInfo> usedInfos,
            IEnumerable<string> readableDescriptionLines)
            : base(() =>
            {
                updateAction();
                return new DependencyUpdateResults(usedInfos);
            })
        {
            UsedInfos = usedInfos;
            ReadableDescriptionLines = readableDescriptionLines;
        }
    }
}
