// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

internal sealed partial class HtmlDocumentSynchronizer
{
    internal readonly struct RazorDocumentVersion(int workspaceVersion, ChecksumWrapper checksum)
    {
        internal int WorkspaceVersion => workspaceVersion;
        internal ChecksumWrapper Checksum => checksum;

        public override string ToString()
            => $"Checksum {checksum} from workspace version {workspaceVersion}";

        internal static async Task<RazorDocumentVersion> CreateAsync(TextDocument razorDocument, CancellationToken cancellationToken)
        {
            var workspaceVersion = razorDocument.Project.Solution.GetWorkspaceVersion();

            var checksum = await razorDocument.GetChecksumAsync(cancellationToken).ConfigureAwait(false);

            return new RazorDocumentVersion(workspaceVersion, checksum);
        }
    }
}
