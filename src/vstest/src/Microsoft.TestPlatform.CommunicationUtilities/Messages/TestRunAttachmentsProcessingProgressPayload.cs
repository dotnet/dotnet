// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

/// <summary>
/// Test run attachments processing complete payload.
/// </summary>
public class TestRunAttachmentsProcessingProgressPayload
{
    /// <summary>
    /// Gets or sets the test run attachments processing complete args.
    /// </summary>
    public TestRunAttachmentsProcessingProgressEventArgs? AttachmentsProcessingProgressEventArgs { get; set; }
}
