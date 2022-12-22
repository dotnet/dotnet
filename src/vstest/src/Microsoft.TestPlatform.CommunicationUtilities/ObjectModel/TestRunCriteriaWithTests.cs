// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;

using Newtonsoft.Json;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

/// <summary>
/// The test run criteria with tests.
/// </summary>
public class TestRunCriteriaWithTests
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteriaWithTests"/> class.
    /// Ensure that names of constructor parameters match the public property names of the same for JSON serialization
    /// </summary>
    /// <param name="tests"> The tests. </param>
    /// <param name="package"> The package which actually contain sources. A testhost can at max execute for one package at time
    /// Package can be null if test source, and package are same
    /// </param>
    /// <param name="runSettings"> The test run settings. </param>
    /// <param name="testExecutionContext"> The test Execution Context. </param>
    [JsonConstructor]
    public TestRunCriteriaWithTests(IEnumerable<TestCase> tests, string? package, string? runSettings, TestExecutionContext testExecutionContext)
    {
        Tests = tests;
        Package = package;
        RunSettings = runSettings;
        TestExecutionContext = testExecutionContext;
    }

    /// <summary>
    /// Gets the tests.
    /// </summary>
    public IEnumerable<TestCase> Tests { get; private set; }

    /// <summary>
    /// Gets the test run settings.
    /// </summary>
    public string? RunSettings { get; private set; }

    /// <summary>
    /// Gets or sets the test execution context.
    /// </summary>
    public TestExecutionContext TestExecutionContext { get; set; }

    /// <summary>
    /// Gets the test Containers (e.g. .appx, .appxrecipie)
    /// </summary>
    public string? Package { get; private set; }
}
