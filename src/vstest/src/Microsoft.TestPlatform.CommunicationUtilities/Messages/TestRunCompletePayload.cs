// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

/// <summary>
/// The test run complete payload.
/// </summary>
public class TestRunCompletePayload
{
    /// <summary>
    /// Gets or sets the test run complete args.
    /// </summary>
    public TestRunCompleteEventArgs? TestRunCompleteArgs { get; set; }

    /// <summary>
    /// Gets or sets the last run tests.
    /// </summary>
    public TestRunChangedEventArgs? LastRunTests { get; set; }

    /// <summary>
    /// Gets or sets the run attachments.
    /// </summary>
    public ICollection<AttachmentSet>? RunAttachments { get; set; }

    /// <summary>
    /// Gets or sets the executor uris that were used to run the tests.
    /// </summary>
    public ICollection<string>? ExecutorUris { get; set; }
}
