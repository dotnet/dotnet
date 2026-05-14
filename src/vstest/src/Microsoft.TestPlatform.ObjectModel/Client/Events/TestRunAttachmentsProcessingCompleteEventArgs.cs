// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

[DataContract]
public class TestRunAttachmentsProcessingCompleteEventArgs : EventArgs
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="isCanceled">Specifies whether the attachments processing is canceled.</param>
    /// <param name="error">Specifies the error encountered during the execution of the attachments processing.</param>
    public TestRunAttachmentsProcessingCompleteEventArgs(bool isCanceled, Exception? error)
    {
        IsCanceled = isCanceled;
        Error = error;
    }

    /// <summary>
    /// Gets a value indicating whether the attachments processing is canceled or not.
    /// </summary>
    [DataMember]
    public bool IsCanceled { get; private set; }

    /// <summary>
    /// Gets the error encountered during the attachments processing of the test runs. Null if there is no error.
    /// </summary>
    [DataMember]
    public Exception? Error { get; private set; }

    /// <summary>
    /// Get or Sets the Metrics (used for telemetry)
    /// </summary>
    [DataMember]
    public IDictionary<string, object>? Metrics { get; set; }
}
