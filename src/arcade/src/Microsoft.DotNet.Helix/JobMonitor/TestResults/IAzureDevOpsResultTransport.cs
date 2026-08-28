// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;

internal interface IAzureDevOpsResultTransport
{
    Task<string> PublishResultsAsync(object results, CancellationToken cancellationToken);

    Task UploadAttachmentAsync(
        long testResultId,
        long? testSubResultId,
        string fileName,
        string stream,
        CancellationToken cancellationToken);
}
