// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;

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
