// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

public class TestHostLaunchedPayload
{
    /// <summary>
    /// Gets or sets the test run process id of test host.
    /// </summary>
    [DataMember]
    public int ProcessId { get; set; }
}
