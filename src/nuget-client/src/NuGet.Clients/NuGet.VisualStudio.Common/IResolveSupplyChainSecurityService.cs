// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace NuGet.VisualStudio
{
    public interface IResolveSupplyChainSecurityService
    {
        /// <summary>
        /// Launches the NuGet supply chain security resolution flow with GitHub Copilot.
        /// </summary>
        /// <param name="source">The surface the flow was launched from.</param>
        /// <param name="prompt">The prompt to send to GitHub Copilot.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        Task LaunchResolveAsync(
            ResolveSupplyChainSecuritySource source,
            string prompt,
            CancellationToken cancellationToken);
    }
}
