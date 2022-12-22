// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;

using Newtonsoft.Json;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

/// <summary>
/// The test run criteria with sources.
/// </summary>
public class TestRunCriteriaWithSources
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteriaWithSources"/> class.
    /// </summary>
    /// <param name="adapterSourceMap"> The adapter source map.  </param>
    /// <param name="package"> The package which actually contain sources. A testhost can at max execute for one package at time
    /// Package can be null if test source, and package are same
    /// </param>
    /// <param name="runSettings"> The run settings.  </param>
    /// <param name="testExecutionContext"> The test Execution Context. </param>
    [JsonConstructor]
    public TestRunCriteriaWithSources(Dictionary<string, IEnumerable<string>> adapterSourceMap, string? package, string? runSettings, TestExecutionContext testExecutionContext)
    {
        AdapterSourceMap = adapterSourceMap;
        Package = package;
        RunSettings = runSettings;
        TestExecutionContext = testExecutionContext;
    }

    /// <summary>
    /// Gets the adapter source map.
    /// </summary>
    public Dictionary<string, IEnumerable<string>> AdapterSourceMap { get; private set; }

    /// <summary>
    /// Gets the run settings.
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
