// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Class used to define the TestRunAttachmentsProcessingPayload sent by the Vstest.console translation layers into design mode
/// </summary>
public class TestRunAttachmentsProcessingPayload
{
    /// <summary>
    /// Collection of attachments.
    /// </summary>
    [DataMember]
    public IEnumerable<AttachmentSet>? Attachments { get; set; }

    /// <summary>
    /// Collection of the invoked data collectors.
    /// </summary>
    [DataMember]
    public IEnumerable<InvokedDataCollector>? InvokedDataCollectors { get; set; }

    /// <summary>
    /// Gets or sets the settings used for the test run request.
    /// </summary>
    [DataMember]
    public string? RunSettings { get; set; }

    /// <summary>
    ///  Gets or sets whether Metrics should be collected or not.
    /// </summary>
    [DataMember]
    public bool CollectMetrics { get; set; }
}
