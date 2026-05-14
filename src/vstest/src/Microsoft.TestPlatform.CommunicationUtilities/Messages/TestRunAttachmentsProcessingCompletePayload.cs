// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

/// <summary>
/// Test run attachments processing complete payload.
/// </summary>
public class TestRunAttachmentsProcessingCompletePayload
{
    /// <summary>
    /// Gets or sets the test run attachments processing complete args.
    /// </summary>
    public TestRunAttachmentsProcessingCompleteEventArgs? AttachmentsProcessingCompleteEventArgs { get; set; }

    /// <summary>
    /// Gets or sets the attachments.
    /// </summary>
    public IEnumerable<AttachmentSet>? Attachments { get; set; }
}
