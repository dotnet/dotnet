// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Options to be passed into the Test Platform during Discovery/Execution.
/// </summary>
[DataContract]
public class TestPlatformOptions
{
    /// <summary>
    /// Gets or sets the filter criteria for test cases.
    /// </summary>
    ///
    /// <remarks>
    /// This is only used when running tests with sources.
    /// </remarks>
    [DataMember]
    public string? TestCaseFilter { get; set; }

    /// <summary>
    /// Gets or sets the filter options if there are any.
    /// </summary>
    ///
    /// <remarks>
    /// This will be valid only if TestCase filter is present.
    /// </remarks>
    [DataMember]
    public FilterOptions? FilterOptions { get; set; }

    /// <summary>
    /// Gets or sets whether metrics should be collected or not.
    /// </summary>
    [DataMember]
    public bool CollectMetrics { get; set; }

    /// <summary>
    /// Gets or sets whether default adapters should be skipped or not.
    /// </summary>
    [DataMember]
    public bool SkipDefaultAdapters { get; set; }
}
