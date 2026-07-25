// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace NuGet.VisualStudio
{
    public interface IFixVulnerabilitiesService
    {
        /// <summary>
        /// Launches the "Fix Vulnerabilities with GitHub Copilot" flow.
        /// </summary>
        /// <param name="source">The surface the flow was launched from.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        Task LaunchFixVulnerabilitiesAsync(FixVulnerabilitiesSource source, CancellationToken cancellationToken);
    }
}
