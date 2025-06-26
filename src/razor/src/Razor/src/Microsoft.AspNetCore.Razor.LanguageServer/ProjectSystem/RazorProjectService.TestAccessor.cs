// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal partial class RazorProjectService
{
    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(RazorProjectService instance)
    {
        public ValueTask WaitForInitializationAsync()
            => instance.WaitForInitializationAsync();

        public async Task<ProjectKey> AddProjectAsync(
            string filePath,
            string intermediateOutputPath,
            RazorConfiguration? configuration,
            string? rootNamespace,
            string? displayName,
            CancellationToken cancellationToken)
        {
            var service = instance;

            await service.WaitForInitializationAsync().ConfigureAwait(false);

            return await instance._projectManager
                .UpdateAsync(
                    updater => service.AddProjectCore(updater, filePath, intermediateOutputPath, configuration, rootNamespace, displayName),
                    cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
