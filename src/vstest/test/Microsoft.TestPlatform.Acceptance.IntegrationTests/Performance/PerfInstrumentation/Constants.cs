// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK
namespace Microsoft.TestPlatform.AcceptanceTests.Performance.PerfInstrumentation;

/// <summary>
/// The constants.
/// </summary>
public class Constants
{
    /// <summary>
    /// The discovery task.
    /// </summary>
    public const string DiscoveryTask = "Discovery";

    /// <summary>
    /// The execution task.
    /// </summary>
    public const string ExecutionTask = "Execution";

    /// <summary>
    /// The execution request task.
    /// </summary>
    public const string ExecutionRequestTask = "ExecutionRequest";

    /// <summary>
    /// The discovery request task.
    /// </summary>
    public const string DiscoveryRequestTask = "DiscoveryRequest";

    /// <summary>
    /// The test host task.
    /// </summary>
    public const string TestHostTask = "TestHost";

    /// <summary>
    /// The vs test console task.
    /// </summary>
    public const string VsTestConsoleTask = "VsTestConsole";

    /// <summary>
    /// The adapter search task.
    /// </summary>
    public const string AdapterSearchTask = "AdapterSearch";

    /// <summary>
    /// The adapter execution task.
    /// </summary>
    public const string AdapterExecutionTask = "AdapterExecution";

    /// <summary>
    /// The adapter discovery task.
    /// </summary>
    public const string AdapterDiscoveryTask = "AdapterDiscovery";

    #region PayLoad Property Names

    /// <summary>
    /// The execution uri property.
    /// </summary>
    public const string ExecutionUriProperty = "executorUri";

    /// <summary>
    /// The number of tests property.
    /// </summary>
    public const string NumberOfTestsProperty = "numberOfTests";
    #endregion

}
#endif
