// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.Extensions.HtmlLogger.ObjectModel;

/// <summary>
/// Stores the list of failed results and list of all results corresponding to the source.
/// </summary>
[DataContract]
public class TestResultCollection
{
    private readonly string _source;

    public TestResultCollection(string source) => _source = source ?? throw new ArgumentNullException(nameof(source));

    /// <summary>
    /// Source of the test dll.
    /// </summary>
    [DataMember]
    public string Source
    {
        get => _source;
        private set { }
    }

    /// <summary>
    /// Hash id of source.
    /// </summary>
    [DataMember]
    public int Id
    {
        get => _source.GetHashCode();
        private set { }
    }

    /// <summary>
    /// List of test results.
    /// </summary>
    [DataMember] public List<TestResult>? ResultList { get; set; }

    /// <summary>
    /// List of failed test results.
    /// </summary>
    [DataMember] public List<TestResult>? FailedResultList { get; set; }
}
