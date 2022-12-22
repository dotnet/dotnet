// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuGet.Commands
{
    public interface IRestoreRequestProvider
    {
        /// <summary>
        /// True if this provider supports the given path. Only one provider should handle an input.
        /// </summary>
        Task<bool> Supports(string path);

        /// <summary>
        /// Create RestoreRequest objects.
        /// </summary>
        /// <param name="inputPath">Project.json or project file path.</param>
        /// <param name="restoreContext">Command line arguments.</param>
        /// <returns></returns>
        Task<IReadOnlyList<RestoreSummaryRequest>> CreateRequests(
            string inputPath,
            RestoreArgs restoreContext);
    }
}
