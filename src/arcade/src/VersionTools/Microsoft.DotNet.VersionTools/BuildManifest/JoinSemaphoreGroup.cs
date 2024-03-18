// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Microsoft.DotNet.VersionTools.BuildManifest
{
    public class JoinSemaphoreGroup
    {
        public string JoinSemaphorePath { get; set; }

        /// <summary>
        /// Names of the semaphores that must all complete to update the join semaphore.
        /// </summary>
        public IEnumerable<string> ParallelSemaphorePaths { get; set; }
    }
}
