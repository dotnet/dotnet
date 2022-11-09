// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Runtime.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Class used to define the start test session criteria.
/// </summary>
[DataContract]
public class StartTestSessionCriteria
{
    /// <summary>
    /// Gets or sets the sources used for starting the test session.
    /// </summary>
    [DataMember]
    public IList<string>? Sources { get; set; }

    /// <summary>
    /// Gets or sets the run settings used for starting the test session.
    /// </summary>
    [DataMember]
    public string? RunSettings { get; set; }

    /// <summary>
    /// Gets or sets the test host launcher used for starting the test session.
    /// </summary>
    [DataMember]
    public ITestHostLauncher? TestHostLauncher { get; set; }
}
