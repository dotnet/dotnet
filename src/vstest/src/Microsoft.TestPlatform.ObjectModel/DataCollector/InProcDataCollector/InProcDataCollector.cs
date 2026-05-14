// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollector.InProcDataCollector;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.InProcDataCollector;

/// <summary>
/// Listener interface for external exe from test host
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Interface is part of the public API.")]
public interface InProcDataCollection
{
    /// <summary>
    /// Initializes the In Process DataCollection with the DataCollectionSink
    /// </summary>
    /// <param name="dataCollectionSink">data collection sink object</param>
    void Initialize(IDataCollectionSink dataCollectionSink);

    /// <summary>
    /// Called when test session starts
    /// </summary>
    /// <param name="testSessionStartArgs">
    /// The test Session Start Args.
    /// </param>
    void TestSessionStart(TestSessionStartArgs testSessionStartArgs);

    /// <summary>
    /// Called when test case starts
    /// </summary>
    /// <param name="testCaseStartArgs">
    /// Test Case start args
    /// </param>
    void TestCaseStart(TestCaseStartArgs testCaseStartArgs);

    /// <summary>
    /// Called when test case end
    /// </summary>
    /// <param name="testCaseEndArgs">
    /// The test Case End Args.
    /// </param>
    void TestCaseEnd(TestCaseEndArgs testCaseEndArgs);

    /// <summary>
    /// Called when test session end
    /// </summary>
    /// <param name="testSessionEndArgs">
    /// The test Session End Args.
    /// </param>
    void TestSessionEnd(TestSessionEndArgs testSessionEndArgs);
}
