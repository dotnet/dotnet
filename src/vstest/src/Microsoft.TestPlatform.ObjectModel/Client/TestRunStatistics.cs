// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Defines the test run stats header
/// </summary>
[DataContract]
public class TestRunStatistics : ITestRunStatistics
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunStatistics"/> class.
    /// </summary>
    /// <remarks>This constructor doesn't perform any parameter validation, it is meant to be used for serialization."/></remarks>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public TestRunStatistics()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        // Default constructor for Serialization.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunStatistics"/> class.
    /// </summary>
    /// <param name="stats"> The stats. </param>
    public TestRunStatistics(IDictionary<TestOutcome, long>? stats)
    {
        Stats = stats;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunStatistics"/> class.
    /// </summary>
    /// <param name="executedTests"> The executed tests. </param>
    /// <param name="stats"> The stats. </param>
    /// <remarks> This constructor is only needed to reconstruct the object during deserialization.</remarks>
    public TestRunStatistics(long executedTests, IDictionary<TestOutcome, long>? stats)
    {
        ExecutedTests = executedTests;
        Stats = stats;
    }

    /// <summary>
    /// Gets or sets the number of tests that have been run.
    /// </summary>
    [DataMember]
    public long ExecutedTests { get; set; }

    /// <summary>
    /// Gets the test stats which is the test outcome versus its state.
    /// </summary>
    [DataMember]
    public IDictionary<TestOutcome, long>? Stats { get; private set; }

    /// <summary>
    /// Gets the number of tests with a specified outcome.
    /// </summary>
    /// <param name="testOutcome"> The test outcome. </param>
    /// <returns> The number of tests with this outcome. </returns>
    public long this[TestOutcome testOutcome]
    {
        get
        {
            return Stats?.TryGetValue(testOutcome, out var count) == true ? count : 0;
        }
    }
}
